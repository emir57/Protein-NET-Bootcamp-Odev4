using AutoMapper;
using Core.Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using Pagination.Business.Abstract;
using Pagination.Dto.Concrete;
using Pagination.Entity.Concrete;

namespace Pagination.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class PersonsController : BaseController<Person, PersonDto>
    {
        private readonly IPersonService _service;
        public PersonsController(IPersonService service, IMapper mapper) : base(service, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Add 1000 rows
        /// </summary>
        /// <returns></returns>
        [NonAction]
        [HttpGet]
        public async Task<IActionResult> Add_1000_rows()
        {
            for (int i = 0; i < 1000; i++)
            {
                await base.AddAsync(new PersonDto
                {
                    FirstName = $"Emir{i}",
                    LastName = $"Gürbüz{i}",
                    Email = $"emir{i}@hotmail.com",
                    Description = "lorem",
                    Phone = "000",
                    DateOfBirth = new DateTime(2002, 9, 8)
                });
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter paginationFilter)
        {
            var result = await _service.GetPaginationAsync(paginationFilter);
            return Ok(result);
        }

        [NonAction]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PersonDto personDto)
        {
            return await base.AddAsync(personDto);
        }

    }
}
