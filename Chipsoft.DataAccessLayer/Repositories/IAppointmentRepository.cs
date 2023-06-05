namespace Chipsoft.Data.Repositories
{
    public interface IAppointmentRepository
    {
        void DeleteById(int id);

        IEnumerable<Appointment> GetAll();

        void Insert(Appointment appointment);

        void Save();

    }
}