using System.Net.NetworkInformation;

namespace API_Payments.Enum
{
    public enum RequestStatusEnum
    {
        Saved = 0,
        BeingProcessed = 1,
        Aproved = 2,
        Rejected = 3
    }

    public static class RequestStatusEnumDescription
    {
        public static string GetDescription(int status)
        {
            string ret = "";

            switch (status)
            {
                case 0:
                    ret = "Saved";
                    break;
                case 1:
                    ret = "Being Processed";
                    break;
                case 2:
                    ret = "Aproved";
                    break;
                case 3:
                    ret = "Rejected";
                    break;
            }
            return ret;
        }
    }
}

