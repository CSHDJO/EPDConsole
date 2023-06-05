namespace Chipsoft.BusinessLogicLayer
{
    public interface IPhysicianService
    {
        void AddAppointment(string naamArts, string naamPatient, string beschrijving);
        void AddPhysician(string naam, string land, string stad, string straat, string huisnummer);
        void DeletePhysician(string naam);
        List<AppointmentItem> ShowAppointment();
        List<AppointmentItem> ShowAppointmentForPhysician(string naam);
    }
}