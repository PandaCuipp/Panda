using System;

namespace CreateNCSql.Code
{
    /// <summary>
    /// 
    /// </summary>
    public class InputDto
    {
        /// <summary>
        /// 应付单主键 100966100000000SLB87
        /// </summary>
        public string YFVouchid { get; set; }
        /// <summary>
        /// 付款单主键 100966100000000SLB87
        /// </summary>
        public string FKVouchid { get; set; }

        /// <summary>
        /// 总无税金额
        /// </summary>
        public decimal NoTaxMoney { get; set; }

        /// <summary>
        /// 税率 13
        /// </summary>
        public decimal TaxRate { get; set; }

        /// <summary>
        /// 单据日期
        /// </summary>
        public DateTime BillDate { get; set; }

        /// <summary>
        /// 随机码
        /// </summary>
        public int RandNo { get; set; }

        /// <summary>
        /// 单位编码  1009
        /// </summary>
        public string Dwbm { get; set; }

        /// <summary>
        /// 录入人 000166100000001HGVJC => 采云  配置：NCOption.DZCreator
        /// </summary>
        public string Creator { get; set; }
    }
}
