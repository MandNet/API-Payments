namespace API_Payments.Enum
{
    public enum CardStatusEnum
    {
        Blocked = 0,
        Authorized = 1,
        Suspicious = 2,
        Canceled = 3
    }

    public static class CardStatusEnumDescription
    {
        public static string GetDescription(int status)
        {
            string ret = "";

            switch (status)
            {
                case 0:
                    ret = "Blocked";
                    break;
                case 1:
                    ret = "Authorized";
                    break;
                case 2:
                    ret = "Suspicious";
                    break;
                case 3:
                    ret = "Canceled";
                    break;
            }
            return ret;
        }
    }
}

