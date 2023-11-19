namespace SADA.ViewModel.Start
{
    public class MockAuthViewModel : AuthViewModel
    {
        public MockAuthViewModel() : base(null, null)
        {
            Login = "User12345";
            Password = "1124234";
        }
    }
}