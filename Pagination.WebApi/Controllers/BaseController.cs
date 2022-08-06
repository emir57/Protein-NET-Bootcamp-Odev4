using AutoMapper;
using Core.Dto;
using Core.Entity;
using Microsoft.AspNetCore.Mvc;
using Pagination.Business.Abstract;

namespace Pagination.WebApi.Controllers
{
    [ApiController]
    public class BaseController<TEntity, TDto> : ControllerBase
        where TEntity : class, IEntity, new()
        where TDto : class, IDto, new()
    {
        protected readonly IAsyncBaseService<TEntity, TDto> Service;
        protected readonly IMapper Mapper;
        public BaseController(IAsyncBaseService<TEntity, TDto> service, IMapper mapper)
        {
            Service = service;
            Mapper = mapper;
        }

        [NonAction]
        public async Task<IActionResult> AddAsync(TDto dto)
        {
            var result = await Service.AddAsync(dto);
            if (result.Success == false)
                return BadRequest(result);
            return Ok(result);
        }
        [NonAction]
        public async Task<IActionResult> UpdateAsync(int id, TDto dto)
        {
            var result = await Service.UpdateAsync(id, dto);
            if (result.Success == false)
                return BadRequest(result);
            return Ok(result);
        }
        [NonAction]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await Service.DeleteAsync(id);
            if (result.Success == false)
                return BadRequest(result);
            return Ok(result);
        }
        [NonAction]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await Service.GetByIdAsync(id);
            if (result.Success == false)
                return BadRequest(result);
            return Ok(result);
        }
        [NonAction]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await Service.GetListAsync();
            if (result.Success == false)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
