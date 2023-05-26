using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using quizapi.Business_Logic_Layer.DTO;
using quizapi.Data_Access_Layer.Entities;
using quizapi.Data_Access_Layer.Repository.Interface;

namespace quizapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class QuestionController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IQuestionListingRepo questionListingRepo;



        //Creating Constructor
        public QuestionController(IMapper mapper, IQuestionListingRepo questionListingRepo)
        {
            this.mapper = mapper;
            this.questionListingRepo = questionListingRepo;
        }


        [HttpPost]
       [Authorize(Roles = "Admin")]
        

        public async Task<IActionResult> Create([FromBody] AddQuestionRequestDTO addQuestionRequestDTO)
        {
            //Map DTO to domain Model          
            var questionentity = mapper.Map<Question>(addQuestionRequestDTO);
            await questionListingRepo.CreateAsync(questionentity);
            //Domain Model to DTO


            return Ok(mapper.Map<QuestionDTO>(questionentity));
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetAll()
        {
            var QuestionEntity = await questionListingRepo.GetAllAsync();

            return Ok(mapper.Map<List<QuestionDTO>>(QuestionEntity));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var questionn = await questionListingRepo.GetByIdAsync(id);
            if (questionn == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<QuestionDTO>(questionn));

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        
        public async Task<IActionResult> Update([FromRoute] int id, UpdateQuestionDTO updateQuestionDTO)
        {
            var questionnn = mapper.Map<Question>(updateQuestionDTO);
            questionnn = await questionListingRepo.UpdateAsync(id, questionnn);
            if (questionnn == null)
            {
                return BadRequest();
            }

            return Ok(mapper.Map<QuestionDTO>(questionnn));
        }

        [HttpDelete]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entityDeleted = await questionListingRepo.DeleteAsync(id);
            if (entityDeleted == null)
            {
                return NotFound();
            }

            return NoContent();
        }



    }
}

