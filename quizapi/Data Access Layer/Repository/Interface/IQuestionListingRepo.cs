using quizapi.Data_Access_Layer.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace quizapi.Data_Access_Layer.Repository.Interface
{
    public interface IQuestionListingRepo
    {
        Task<Question> CreateAsync(Question question);
        Task<List<Question>> GetAllAsync();
        Task<Question> GetByIdAsync(int id);
        Task<Question> UpdateAsync(int id, Question question);
        Task<Question> DeleteAsync(int id);
    }
}
