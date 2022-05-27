// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT License.

using Orleans;

await Host.CreateDefaultBuilder(args)
    .UseOrleans(
        (context, builder) =>
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                builder.UseLocalhostClustering()
                    .AddMemoryGrainStorage("shopping-cart")
                    .AddStartupTask<SeedProductStoreTask>();
            }
            else
            {
                const string connectionString = "DefaultEndpointsProtocol=https;AccountName=orleansshoppingstorage;AccountKey=4IjEOzVQeDsLW0wKstPKT/qhpio3seRCrQTSQ8b1zYbc+lkYXkd7ez8U7udz/TpVSUXQWilQIPAe+AStHplmPw==;EndpointSuffix=core.windows.net";
                builder.Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "shopping-cart";
                    options.ServiceId = "shopping-cart";
                })
                .UseAzureStorageClustering(
                    options => options.ConfigureTableServiceClient(connectionString))
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000);
                builder.AddAzureTableGrainStorage(
                    "shopping-cart",
                    options => options.ConfigureTableServiceClient(connectionString));

                //var endpointAddress =
                //    IPAddress.Parse(context.Configuration["WEBSITE_PRIVATE_IP"]);
                //var strPorts =
                //    context.Configuration["WEBSITE_PRIVATE_PORTS"].Split(',');
                //if (strPorts.Length < 2)
                //    throw new Exception("Insufficient private ports configured.");
                //var (siloPort, gatewayPort) =
                //    (int.Parse(strPorts[0]), int.Parse(strPorts[1]));
                //var connectionString =
                //    context.Configuration["ORLEANS_AZURE_STORAGE_CONNECTION_STRING"];

                //builder
                //    .ConfigureEndpoints(endpointAddress, siloPort, gatewayPort)
                //    .Configure<ClusterOptions>(
                //        options =>
                //        {
                //            options.ClusterId = "ShoppingCartCluster";
                //            options.ServiceId = nameof(ShoppingCartService);
                //        }).UseAzureStorageClustering(
                //    options => options.ConfigureTableServiceClient(connectionString));
                //builder.AddAzureTableGrainStorage(
                //    "shopping-cart",
                //    options => options.ConfigureTableServiceClient(connectionString));
            }
        })
    .ConfigureWebHostDefaults(
        webBuilder => webBuilder.UseStartup<Startup>())
    .RunConsoleAsync();


//await Host.CreateDefaultBuilder(args)
//    .UseOrleans(
//        (context, builder) =>
//        {
//            builder.UseLocalhostClustering()
//                    .AddMemoryGrainStorage("shopping-cart")
//                    .AddStartupTask<SeedProductStoreTask>();
//        })
//    .ConfigureWebHostDefaults(
//        webBuilder => webBuilder.UseStartup<Startup>())
//    .RunConsoleAsync();