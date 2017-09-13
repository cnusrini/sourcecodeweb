using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Employee
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string email { get; set; }
        [JsonProperty("modeSupported")]
        public string mode_supported { get; set; }
        [JsonProperty("employeeType")]
        public string emp_type { get; set; }
        [JsonProperty("workerName")]
        public string worker_name { get; set; }
        [JsonProperty("workBeginWeek")]
        public string work_begin_week { get; set; }
        [JsonProperty("timeHoursSpent")]
        public Nullable<int> time_hours_spent { get; set; }
        [JsonProperty("partTimeSpentHour")]
        public Nullable<int> part_time_spent_hour { get; set; }
        [JsonProperty("totalWorkingHour")]
        public Nullable<int> total_working_hour { get; set; }
        [JsonProperty("totalRemaingHour")]
        public Nullable<int> total_remaing_hour { get; set; }
        [JsonProperty("lunchTime")]
        public string lunch_time { get; set; }
        [JsonProperty("normalWorkingHour")]
        public string normal_working_hour { get; set; }
        [JsonProperty("emergencyBreakHour")]
        public string emergency_break_hour { get; set; }
        [JsonProperty("plannedLeave")]
        public string planned_leave { get; set; }
        [JsonProperty("ifWorkingWeekend")]
        public Nullable<bool>if_working_weekend { get; set; }
        [JsonProperty("ifComplianceVoilation")]
        public Nullable<bool> if_compliance_voilation { get; set; }
        [JsonProperty("workerAuthorization")]
        public string worker_authorization { get; set; }
        [JsonProperty("supervisorApprovalStatus")]
        public string supervisor_approval_status { get; set; }
    }
}