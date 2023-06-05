
namespace Chipsoft.Data.Repositories
{
    public interface IPatientRepository
    {
        void DeleteById(int id);

        IEnumerable<Patient> GetAll();

        void Insert(Patient patient);

        void Save();
    }
}
