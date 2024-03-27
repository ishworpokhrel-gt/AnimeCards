using Sieve.Services;

namespace AnimeCards.Extension.SieveConfiguration.Anime
{
    public class AnimeSieveExtension : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Entity.Anime>(a => a.Id)
                .CanFilter()
                .CanSort();

            mapper.Property<Entity.Anime>(a => a.Name)
                .CanFilter()
                .CanSort();

            mapper.Property<Entity.Anime>(a => a.Language)
                .CanFilter()
                .CanSort();

            mapper.Property<Entity.Anime>(a => a.RatingLevel)
                .CanFilter()
                .CanSort();
        }
    }
}
