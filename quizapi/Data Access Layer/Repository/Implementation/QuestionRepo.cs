using quizapi.Data_Access_Layer.context;
using quizapi.Data_Access_Layer.Entities;
using quizapi.Data_Access_Layer.Repository.Interface;
using Microsoft.EntityFrameworkCore;



namespace quizapi.Data_Access_Layer.Repository.Implementation
{

    public class QuestionRepo : IQuestionListingRepo
    {
        private readonly quizdbcontext dbContext;

        public QuestionRepo(quizdbcontext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Question> CreateAsync(Question question)
        {
            await dbContext.Questions.AddAsync(question);
            await dbContext.SaveChangesAsync();
            return question;
        }

        public async Task<List<Question>> GetAllAsync()
        {
            return await dbContext.Questions.ToListAsync();
        }

        public async Task<Question> GetByIdAsync(int id)
        {
            return await dbContext.Questions.FirstOrDefaultAsync(x => x.QnId == id);
        }
        public async Task<Question> UpdateAsync(int id, Question question)
        {
            var existingQuestion = await dbContext.Questions.FirstOrDefaultAsync(x => x.QnId == id);
            if (existingQuestion == null)
            {
                return null;
            }

            existingQuestion.QnId= question.QnId;
            existingQuestion.QnInWords = question.QnInWords;
            existingQuestion.Option1= question.Option1;
            existingQuestion.Option2 = question.Option2;
            existingQuestion.Option3 = question.Option3;
            existingQuestion.Option4 = question.Option4;



            await dbContext.SaveChangesAsync();
            return existingQuestion;
        }
        public async Task<Question> DeleteAsync(int id)
        {
            var existingQuestion = await dbContext.Questions.FirstOrDefaultAsync(x => x.QnId == id);
            if (existingQuestion == null)


            {
                return null;
            }
            dbContext.Questions.Remove(existingQuestion);

            await dbContext.SaveChangesAsync();
            return existingQuestion;
        }
    }
}
