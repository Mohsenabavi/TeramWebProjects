using System.ComponentModel;

namespace Teram.HR.Module.Recruitment.Enums
{
    public enum IntroductionLetterType
    {
        [Description("معرفی نامه طب کار")]
        OccupationalMedicine = 1,

        [Description("معرفی نامه آزمایش عدم سوء مصرف مواد")]
        NoAddiction = 2,

        [Description("نامه توضیحات")]
        Document = 3
    }
}
