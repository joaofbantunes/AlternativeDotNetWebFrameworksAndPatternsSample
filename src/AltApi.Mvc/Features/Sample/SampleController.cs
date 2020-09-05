using System;
using AltApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AltApi.Mvc.Features.Sample
{
    [ApiController]
    [Route("sample")]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        public ActionResult<SampleOutputModel> Get()
            => new SampleOutputModel
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

        [HttpPost]
        public ActionResult<SampleOutputModel> Post(SampleInputModel input)
            => new SampleOutputModel
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

        // TODO: an endpoint going to the DB, or something else involving async stuff?
    }
}