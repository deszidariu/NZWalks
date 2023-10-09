using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        // create walk
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var walkDomaniModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomaniModel);

            return Ok(mapper.Map<WalkDto>(walkDomaniModel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walkDomainModels = await walkRepository.GetAllAsync();

            return Ok(mapper.Map<List<WalkDto>>(walkDomainModels));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomaniModel = await walkRepository.GetByIdAsync(id);

            if (walkDomaniModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkDomaniModel));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalk)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var walkDomainModel = mapper.Map<Walk>(updateWalk);

            var walkDomaniModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomaniModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkDomaniModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
           var deletedWalk = await walkRepository.DeleteAsync(id);

            if(deletedWalk == null)
            {
                return NotFound(nameof(deletedWalk));
            }

            return Ok(mapper.Map<WalkDto>(deletedWalk));
        }
    }
}
