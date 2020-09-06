using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

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
                    (
                        SomeId: Guid.NewGuid(),
                        SomeDescription: nameof(SampleOutputModel.SomeDescription),
                        SomeDate: DateTime.UtcNow,
                        SomeInnerModel: new SampleInnerModel
                        (
                            SomeInnerId: Guid.NewGuid(),
                            SomeInnerDate: DateTime.UtcNow,
                            SomeInnerDescription: nameof(SampleInnerModel.SomeInnerDescription)
                        )
                    );

                    ctx.Response.StatusCode = (int)HttpStatusCode.OK;
                    await ctx.Response.WriteAsJsonAsync(output);
                });

                endpoints.MapPost("/sample", async ctx =>
                {
                    var input = await ctx.Request.ReadFromJsonAsync<SampleInputModel>();
                    var output = new SampleOutputModel
                    (
                        SomeId: input.SomeId,
                        SomeDescription: input.SomeDescription,
                        SomeDate: input.SomeDate,
                        SomeInnerModel: new SampleInnerModel
                        (
                            SomeInnerId: input.SomeInnerModel.SomeInnerId,
                            SomeInnerDate: input.SomeInnerModel.SomeInnerDate,
                            SomeInnerDescription: input.SomeInnerModel.SomeInnerDescription
                        )
                    );

                    ctx.Response.StatusCode = (int)HttpStatusCode.OK;
                    await ctx.Response.WriteAsJsonAsync(output);
                });
            });
        });
    })
    .Build()
    .Run();

public record SampleInputModel(Guid SomeId, string SomeDescription, DateTime SomeDate, SampleInnerModel SomeInnerModel);

public record SampleOutputModel(Guid SomeId, string SomeDescription, DateTime SomeDate, SampleInnerModel SomeInnerModel);

public record SampleInnerModel(Guid SomeInnerId, string SomeInnerDescription, DateTime SomeInnerDate);