using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                if (grpcClient != null)
                {
                    var platforms = grpcClient.ReturnAllPlatforms();

                    SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>()!, platforms!);
                }
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platoforms)
        {
            Console.WriteLine("Seeging new platforms...");

            foreach (var platform in platoforms)
            {
                if (!repo.ExternalPlatformExists(platform.ExternalID))
                {
                    repo.CreatePlatform(platform);
                }
                repo.SaveChanges();
            }
        } 
    }
}