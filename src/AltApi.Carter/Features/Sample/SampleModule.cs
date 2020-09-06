using System;
using Carter;
using Carter.ModelBinding;
using Carter.Response;

namespace AltApi.Carter.Features.Sample
{
    public record SampleInputModel(Guid SomeId, string SomeDescription, DateTime SomeDate, SampleInnerModel SomeInnerModel);

    public record SampleOutputModel(Guid SomeId, string SomeDescription, DateTime SomeDate, SampleInnerModel SomeInnerModel);

    public record SampleInnerModel(Guid SomeInnerId, string SomeInnerDescription, DateTime SomeInnerDate);

    public class SampleModule : CarterModule
    {
        public SampleModule()
        {
            Get("/sample", async ctx =>
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

                await ctx.Response.Negotiate(output);
            });

            Post("/sample", async ctx =>
            {
                var input = await ctx.Request.Bind<SampleInputModel>();

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

                await ctx.Response.Negotiate(output);
            });
        }
    }
}