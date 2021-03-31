using System;
using System.Collections.Generic;

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
        /// 付款单 单据编号 100966100000000SLB87
        /// </summary>
        public string FKBillNo { get; set; }

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
        /// 上级公司主键   select fathercorp from bd_corp where pk_corp = '1009'
        /// </summary>
        public string DwbmParentId { get; set; }

        /// <summary>
        /// 单位编码  1009
        /// </summary>
        public string Dwbm { get; set; }

        /// <summary>
        /// 单位编码  SZN
        /// </summary>
        public string DwbmCode { get; set; }

        /// <summary>
        /// 录入人 000166100000001HGVJC => 采云  配置：NCOption.DZCreator
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 实时凭证主键 102266100000000VDLJU
        /// </summary>
        public string DapRtvouchKey { get; set; }

        /// <summary>
        /// 单据数量
        /// </summary>
        public int DanJuNum { get; set; }

        /// <summary>
        /// 会计主体 0001O110000000000F7R
        /// </summary>
        public string GlorgKey { get; set; }

        /// <summary>
        /// 财务账簿  0001AA10000000004VTR
        /// </summary>
        public string GlbookKey { get; set; }

        /// <summary>
        /// 物料信息
        /// </summary>
        public List<GoodsCost> GoodsList { get; set; }

        /// <summary>
        /// 本币汇率
        /// </summary>
        public decimal HuiLv { get; set; }
    }
}

