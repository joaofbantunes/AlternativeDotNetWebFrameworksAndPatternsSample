using System;
using System.Net;
using System.Text.Json;
using AltApi.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace AltApi.RouteToCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.Configure((webHostBuilderContext, app) =>
                    {
                        if (webHostBuilderContext.HostingEnvironment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }

                        app.UseRouting();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapGet("/sample", async ctx =>
                            {
                                var output = new SampleOutputModel
                                {
                                    SomeId = Guid.NewGuid(),
                                    SomeDescription = nameof(SampleOutputModel.SomeDescription),
                                    SomeDate = DateTime.UtcNow,
                                    SomeInnerModel = new SampleInnerModel
                                    {
                                        SomeInnerId = Guid.NewGuid(),
                                        SomeInnerDate = DateTime.UtcNow,
                                        SomeInnerDescription = nameof(SampleInnerModel.SomeInnerDescription)
                                    }
                                };

                                await ctx.Response.WriteAsync(JsonSerializer.Serialize(output));
                                ctx.Response.StatusCode = (int)HttpStatusCode.OK;
                            });
                            
                            endpoints.MapPost("/sample", async ctx =>
                            {
                                var input = await JsonSerializer.DeserializeAsync<SampleInputModel>(ctx.Request.Body);
                                var output = new SampleOutputModel
                                {
                                    SomeId = input.SomeId,
                                    SomeDescription = input.SomeDescription,
                                    SomeDate = input.SomeDate,
                                    SomeInnerModel = new SampleInnerModel
                                    {
                                        SomeInnerId = input.SomeInnerModel.SomeInnerId,
                                        SomeInnerDate = input.SomeInnerModel.SomeInnerDate,
                                        SomeInnerDescription = input.SomeInnerModel.SomeInnerDescription
                                    }
                                };

                                await ctx.Response.WriteAsync(JsonSerializer.Serialize(output));
                                ctx.Response.StatusCode = (int)HttpStatusCode.OK;
                            });
                        });
                    });
                })
                .Build()
                .Run();
        }
    }
}
