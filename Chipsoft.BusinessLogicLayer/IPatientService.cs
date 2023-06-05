namespace Chipsoft.BusinessLogicLayer
{
    public interface IPatientService
    {  
        void AddPatient(string naam, string contactgegeven, string land, string stad, string straat, string huisnummer);
        void DeletePatient(string naam);
        List<AppointmentItem> ShowAppointmentForPatient(string naam);
    }
}