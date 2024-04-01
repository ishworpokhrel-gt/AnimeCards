using Common_Shared.ResponseResult;
using Common_Shared.SieveExtensio;
using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Anime;
using Models.PaginationModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Business.Anime
{
    public class AnimeService : IAnimeSerivice
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISieveService _sieveService;
        public AnimeService(AppDbContext dbContext
                             , IWebHostEnvironment webHostEnvironment
                              , ISieveService sieveService)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
            _sieveService = sieveService;
        }

        public async Task<ResponseResult> CreateAnimeAsync(CreateAnimeRequestModel model)
        {
            var existData = await _dbContext.Animes
                                                .Where(a => !a.IsDeleted)
                                                .ToListAsync();
            if (existData.Any(a => a.Name.ToLower() == model.Name.ToLower()))
            {
                return ResponseResult.Failed("Name already exists. Try different one.");
            }
            var addData = new Entity.Anime
            {
                Name = model.Name,
                Language = model.Language,
                RatingLevel = model.RatingLevel,
            };
            addData.ImageUrl = GetImagePath(model.Image);

            await _dbContext.Animes.AddAsync(addData);
            await _dbContext.SaveChangesAsync();

            return ResponseResult.Success("Anime created successfully.");


        }

        public async Task<ResponseResult> GetAllAnimeAsync(PaginationRequestModel model)
        {
            var storedData = _dbContext.Animes
                                            .Where(a => !a.IsDeleted)
                                            .AsQueryable();

            if (storedData == null)
            {
                return ResponseResult.Failed("Empty Record , Not found.");
            }

            var (result, totalCount, totalPage) = await _sieveService.ApplyPagination(storedData, model);

            var data = result.Select(a => new GetAllAnimeResppnseModel
            {
                Id = a.Id,
                Name = a.Name,
                RatingLevel = a.RatingLevel,
                Language = a.Language,
                ImageUrl = a.ImageUrl
            });

            return ResponseResult.Success(data);
        }


        public async Task<ResponseResult> GetAllAnimeByIdAsync(string Id)
        {
            var data = await _dbContext.Animes
                                             .Where(a => a.Id == Id && !a.IsDeleted)
                                             .FirstOrDefaultAsync();
            if (data == null)
            {
                return ResponseResult.Failed("Empty Record , Not found.");
            }

            var individualData = new GetAnimeByIdResponseModel
            {
                Name = data.Name,
                RatingLevel = data.RatingLevel,
                Language = data.Language,
                ImageUrl = data.ImageUrl
            };

            return ResponseResult.Success(individualData);
        }

        public async Task<ResponseResult> UpdateAnimeAsync(string Id, UpdateAnimeRequestModel model)
        {
            var data = await _dbContext.Animes
                                            .Where(a => a.Id == Id && !a.IsDeleted)
                                            .FirstOrDefaultAsync();
            var existDataList = await _dbContext.Animes
                                                .Where(a => !a.IsDeleted)
                                                .ToListAsync();
            if (data == null)
            {
                return ResponseResult.Failed("Empty Record , Not found.");
            }

            if (model.Name != data.Name)
            {
                bool isDuplicate = existDataList.Exists(a => a.Name.ToLower() == model.Name.ToLower());
                if (isDuplicate)
                {
                    return ResponseResult.Failed("Name already exists. Try different one.");
                }
            }


            data.ImageUrl = GetImagePath(model.Image);
            data.Name = model.Name ?? data.Name;
            data.Language = model.Language ?? data.Language;
            data.RatingLevel = model.RatingLevel;

            _dbContext.Animes.Update(data);
            await _dbContext.SaveChangesAsync();
            return ResponseResult.Success("Data Updated Successfully.");
        }
        public async Task<ResponseResult> DeleteAnimeAsync(string Id)
        {
            var data = await _dbContext.Animes
                                           .Where(a => a.Id == Id && !a.IsDeleted)
                                           .FirstOrDefaultAsync();
            if (data == null)
            {
                return ResponseResult.Failed("Empty Record , Not found.");
            }

            data.IsDeleted = true;
            var guid = Guid.NewGuid().ToString();
            data.Name = $"--Deleted{guid}{data.Name}";
            _dbContext.Animes.Update(data);
            await _dbContext.SaveChangesAsync();

            return ResponseResult.Success("Data Deleted Successfully.");
        }

        private string GetImagePath(IFormFile? file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string Url = "";
            if (file != null)
            {
                string FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string Imagepath = Path.Combine(wwwRootPath, @"Images\Animeimage");

                using (var fileStream = new FileStream(Path.Combine(Imagepath, FileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return Url = @"\images\Animeimage\" + FileName;
            }
            else return string.Empty;
        }

        public async Task<Tuple<bool, byte[], string>> ExportAnimeExcelAsync()
        {
            try
            {
                var items = await _dbContext.Animes
                                            .Where(a => !a.IsDeleted)
                                            .ToListAsync();
                var fileName = "AnimeReport";
                var excelOutput = GenerateExcel(items);
                return Tuple.Create(true, excelOutput, fileName);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, Array.Empty<byte>(), string.Empty);
            }
        }

        private static byte[] GenerateExcel(List<Entity.Anime> items)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (var pck = new ExcelPackage())
            {
                var sheet = pck.Workbook.Worksheets.Add("AnimeSheet");
                var range = sheet.Cells["A1"].LoadFromCollection(items, c => c.PrintHeaders = true);
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells.AutoFitColumns();
                return pck.GetAsByteArray();
            }
        }
    }
}
