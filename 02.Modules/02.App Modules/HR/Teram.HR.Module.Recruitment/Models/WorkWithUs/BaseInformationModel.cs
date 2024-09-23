using Microsoft.AspNetCore.Mvc.Rendering;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Framework.Core.Extensions;
using System.Reflection;
using Teram.Web.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using Teram.Framework.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.WorkWithUs
{
    public class BaseInformationModel : ModelBase<BaseInformation, int>
    {
        public int BaseInformationId { get; set; }

        [GridColumn(nameof(Name))]
        [ExportToExcel("نام")]
        public string Name { get; set; }

        [GridColumn(nameof(Lastname))]
        [ExportToExcel("نام خانوادگی")]
        public string Lastname { get; set; }

        [ExportToExcel("نام پدر")]
        public string FatherName { get; set; }

        [ExportToExcel("شماره شناسنامه")]
        public string? IdentityNumber { get; set; }


        [ExportToExcel("سریال شناسنامه")]
        public string? IdentitySerialNumber { get; set; }

        [GridColumn(nameof(NationalCode))]
        [ExportToExcel("کد ملی")]
        public string NationalCode { get; set; }
        public GenderType Gender { get; set; }


        [ExportToExcel("جنسیت")]
        public string GenderTitle => (Gender > 0) ? Gender.GetDescription() : "-";

        [ExportToExcel("شماره تلفن")]
        public string? Phone { get; set; }


        [ExportToExcel("شماره همراه")]
        public string Mobile { get; set; }


        [ExportToExcel("کد پستی")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "شهر محل تولد را وارد نمایید")]
        public int? BirthLocationId { get; set; }

        [ExportToExcel("محل تولد")]
        public string? BirthLocationName { get; set; }

        [ExportToExcel("آدرس")]
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }



        public DateTime? StartDateMilitaryService { get; set; }

        [ExportToExcel("تاریخ شروع نظام وظیفه")]
        public string PersianStartDateMilitaryService => (StartDateMilitaryService != null) ? StartDateMilitaryService.Value.ToPersianDate() : "-";

        public DateTime? EndDateMilitaryService { get; set; }

        [ExportToExcel("تاریخ پایان نظام وظیفه")]
        public string PersianEndDateMilitaryService => (EndDateMilitaryService != null) ? EndDateMilitaryService.Value.ToPersianDate() : "-";

        public MilitaryServiceStatus? MilitaryServiceStatus { get; set; }

        [Required]
        public string? MedicalExemptionReason { get; set; }

        [ExportToExcel("وضعیت نظام وظیفه")]
        public string MilitaryServiceStatusTitle => (MilitaryServiceStatus > 0) ? MilitaryServiceStatus.GetDescription() : "-";

        [ExportToExcel("تعداد ماه سابقه بیمه")]
        public string? InsuranceMonths { get; set; }
        public BloodGroup? BloodGroup { get; set; }


        [ExportToExcel("گروه خونی")]
        public string BloodGroupTitle => (BloodGroup > 0) ? BloodGroup.GetDescription() : "-";

        public MaritalStatus MarriageStatus { get; set; }

        [ExportToExcel("وضعیت تاهل")]
        public string MarriageStatusTilte => (MarriageStatus > 0) ? MarriageStatus.GetDescription() : "-";

        [ExportToExcel("ایمیل")]
        public string? Email { get; set; }


        [ExportToExcel("شماره بیمه")]
        public string? InsuranceNumber { get; set; }
        public int? HomeCityId { get; set; }
        public DateTime CreatedOn { get; set; }

        [ExportToExcel("ملیت")]
        public string Nationality { get; set; }

        [ExportToExcel("تابعیت")]
        public string Citizenship { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public EducationGrade? PartnerEducationLevel { get; set; }


        [ExportToExcel("سطح تحصیلات همسر")]
        public string PartnerEducationLevelTitle => (PartnerEducationLevel > 0) ? PartnerEducationLevel.GetDescription() : "-";


        [ExportToExcel("شغل همسر")]
        public string? PartnerJob { get; set; }


        [ExportToExcel("تعداد فرزندان")]
        public int ChildCount { get; set; }

        public bool HasWorkingRelatives { get; set; }


        public string HasWorkingRelativesText => HasWorkingRelatives ? "بله" : "خیر";

        [Required]
        public string? WorkingRelatives { get; set; }

        [ExportToExcel("عنوان پایان نامه")]
        public string? ThesisTitle { get; set; }

        [ExportToExcel("موفقیت های ویژه تحصیلی")]
        public string? SpecialAcademicAchievements { get; set; }

        [ExportToExcel("موفقیت های ویژه کاری")]
        public string? SpecialWorkSuccesses { get; set; }

        [ExportToExcel("اوقات فراغت")]
        public string? FreeTimeActivities { get; set; }
        public bool HasSpecialDisease { get; set; }

        [ExportToExcel("وضعیت بیماری خاص")]
        public string HasSpecialDiseaseText => HasSpecialDisease ? "بله" : "خیر";

        [Required]

        [ExportToExcel("بیماری خاص")]
        public string? SpecialDisease { get; set; }
        public bool HasCriminalRecord { get; set; }

        [Required]
        public string? CriminalRecord { get; set; }

        [ExportToExcel("وضعیت سابقه کیفری")]
        public string HasCriminalRecordText => HasCriminalRecord ? "بله" : "خیر";


        public bool CanWorkInShifts { get; set; }

        [ExportToExcel("توانایی کار در شیفت")]
        public string CanWorkInShiftsText => CanWorkInShifts ? "بله" : "خیر";

        public bool BusinessMissionAbility { get; set; }

        [ExportToExcel("توانایی ماموریت کاری")]
        public string BusinessMissionAbilityText => BusinessMissionAbility ? "بله" : "خیر";

        public bool HasIntentionToMigrate { get; set; }

        [ExportToExcel("قصد مهاجرت")]
        public string HasIntentionToMigrateText => HasIntentionToMigrate ? "بله" : "خیر";

        public DateTime? ReadyToWorkDate { get; set; }

        [ExportToExcel("تاریخ آمادگی برای شروع به کار")]
        public string PersianReadyToWorkDate => (ReadyToWorkDate.HasValue) ? ReadyToWorkDate.Value.ToPersianDate() : "";
        public List<SelectListItem>? Provinces { get; set; }
        public bool IsAgreed { get; set; }

        public bool IsShow { get; set; }

        public bool HasAlreadyRegistered { get; set; }



        [ExportToExcel("تاریخ تولد")]
        public string BirthPersianDate
        {
            get
            {
                return BirthDate?.ToPersianDate();
            }
        }

        public string? CurrentJobActivity { get; set; }

        public decimal? CurrntSalary { get; set; }

        public bool HasWorkingRelativeInPackingCompanies { get; set; }

        public string HasWorkingRelativeInPackingCompaniesText => HasWorkingRelativeInPackingCompanies ? "بله" : "خیر";

        [Required]
        public string? WorkingRelativeInPackingCompanyName { get; set; }

        public int JobApplicantId { get; set; }

        public FlowType? FlowType { get; set; }
        public List<ResumeModel>? Resumes { get; set; }

        public List<EducationModel>? Educations { get; set; }

        public List<TrainingRecordModel>? TrainingRecords { get; set; }

        public List<EmergencyContactModel>? EmergencyContacts { get; set; }

        public List<PersonnelLanguageModel>? PersonnelLanguages { get; set; }

        public List<PersonnelComputerSkillModel>? PersonnelComputerSkills { get; set; }

    }
}
