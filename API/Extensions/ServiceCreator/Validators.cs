namespace Application.Extensions.ServiceCreator
{
    public static class Validators
    {
        public static bool IsViewMdelInterfaceEnded(string line)
        {
            return line.Contains('}');
        }

        public static bool IsViewModelInterfaceMet(string line)
        {
            return line.Contains("export interface") && line.Contains("ViewModel");
        }
    }
}
