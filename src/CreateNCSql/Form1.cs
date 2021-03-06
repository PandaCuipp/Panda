﻿using CreateNCSql.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CreateNCSql
{
    public partial class Form1 : Form
    {
        private int NO = 1000;

        public Form1()
        {
            InitializeComponent();
        }

        //自动赋值
        private void button2_Click(object sender, EventArgs e)
        {
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtTotalMoney.Text = "2000";
            txtNo.Text = NO.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //编号 4位，防止主键重复
            var randNo = int.Parse(txtNo.Text.Trim());
            //单据日期
            var date = DateTime.Parse(txtDate.Text.Trim());
            var dateStr = $"{date:yyyy-MM-dd}";
            var endDate = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);


            var input = new InputDto()
            {
                BillDate = date,
                Dwbm = "1009",
                DwbmParentId = "1002",
                DwbmCode = "SZN",
                NoTaxMoney = decimal.Parse(txtTotalMoney.Text.Trim()),
                RandNo = randNo,
                TaxRate = 13,
                Creator = "000166100000001HGVJC",
                DanJuNum = 5,
                HuiLv = 1,
            };

            input.YFVouchid = $"{input.Dwbm}66{input.BillDate:yyyyMMdd}{randNo}FE";
            input.FKVouchid = $"{input.Dwbm}{input.BillDate:yyyyMMdd}{randNo}EF";
            input.FKBillNo = $"FK{input.DwbmCode}{date:yyMMdd}0{randNo}";
            var settlementKey = $"{input.Dwbm}66{input.BillDate:yyyyMMdd}{randNo}AF"; //100966100000000SLAV3

            //物料价格明细
            var goodsList = GetGoodsCost(input);
            input.GoodsList = goodsList;

            #region param
            var _last = $"{input.BillDate:yyyyMMdd}{input.RandNo}";

            //无税总额 保留两位小数
            var noTaxTotal = input.NoTaxMoney;

            //税率
            var shuilv = input.TaxRate;
            //kslb			 扣税类别   1：应税外加  （TODO：//0：应税内含 2：不计税）
            var kslb = 1;

            //dwbm 单位编码 1107 => 采云 [组织架构表.UniqueCode]  1009 深圳智能
            var dwbm = input.Dwbm;

            //含税金额
            var taxTotal = goodsList.Sum(x => x.TaxTotal);
            var taxTotalStr = $"{taxTotal:F2}"; //Math.Round(taxTotal, 2, MidpointRounding.AwayFromZero)


            //汇率
            var huilv = input.HuiLv;
            //币种编码
            var bizhong = "00010000000000000001";

            //ywbm 单据类型 select djlxoid from arap_djlx where djdl='yf' and DJLXBM='D1' and DWBM='[公司账套编码]' and dr = 0; 
            var ywbm_yf = "0001O11000000002CUYJ";
            var ywbm_fk = "0001O11000000002TBQ5";


            //采购部门 deptid  部门pk  部门.UniqueCode 1009O1100000000GCYO1 
            var deptid = "1009O1100000000GCYO1";

            //附件 发票数量
            var fjNum = input.DanJuNum;

            //录入人 000166100000001HGVJC => 采云  配置：NCOption.DZCreator
            var creator = "000166100000001HGVJC";

            //其他_备注
            var otherRemark = "其他_备注";

            //提单地编号 0001O1100000000FMC20 = 深圳
            var address = "0001O1100000000FMC20";
            //是否研发 0001O11000000002S8KN 否（ 0001O11000000002S8KM是）
            var yafa = "0001O11000000002S8KN";

            //报销金额
            var baoxiaoJinE = taxTotal;

            //100966100000000SLB87
            var yf_vouchid = input.YFVouchid;
            var fk_vouchid = input.FKVouchid;

            var yfDanJuHao = $"YF{date:yyMMdd}{randNo}";
            //付款单单据号
            var fkDanJuHao = input.FKBillNo;

            //报销事由
            var bxsy = $"报销事由:vouchid={yf_vouchid}";

            //前期已付款 zyx7
            var yifu = 0;

            //hbbm 伙伴编码，客商编码 => 采云 供应商.SupplierNCPK  0001O1100000000209N1 
            var hbbm = "0001O1100000000209N1";

            //jobid 专项基本档案id NULL  0001O11000000002KEVV
            var jobid = "NULL";

            //ywybm 业务员pk select * from bd_psndoc where pk_psndoc  ='000166100000001HTLUV' => 采云 EmployeeLatestInfo.UniqueCode
            var ywybm = "1009A8100000000001DK";

            //摘要
            var zy = "SHL 华强方特（深圳）互联科技有限公司 | WHHQZN-024 1号仓库 | 芜湖市鸠江区赤铸山东路华强文化科技产业园动漫楼芜湖智能工厂 | 谢嗣峰 | 17775298975";

            //======付款单=====

            //合同金额 结算金额
            var hetongjine = taxTotal;

            //前期已报销
            var yibaoxiao = 0;
            //实付金额
            var shifujine = taxTotal;

            //付款协议主键 zyx30 0001O110000000008L57（关联付款协议表bd_payterm主键）=> 采云 SupplierPaymentWay表.NCPK
            var fukuanxieyi = "0001O110000000008L4S";

            //输入合同号 合同号
            var contractno = "21010569";

            //订单号
            var ddh = "";

            //供应商 付款银行名称
            var bankName = "工商银行安溪县支行";
            var bankNumber = "11076610000000009NES"; //银行账户信息主键

            //对方账户主键
            var gongYingShangBankKey = "0001O110000000020CXC";

            //fph 发票维护单据号 My_FaPiaoWeiHu_003
            var fph = $"My_FaPiaoWeiHu_{randNo}";

            //结算主键
            var pk_settlement = $"C-JS{_last}";

            //setlleno 结算号 CMP@210125000005
            var setlleno = $"CMP@{DateTime.Now:yyMMdd}{randNo}";

            //0001O11000000008F0B7
            var F0B7 = "0001O11000000008F0B7";


            #endregion


            var sb = new StringBuilder();
            #region 应付单 主表sql

            sb.AppendLine(@$"
-- 应付单 主表
INSERT INTO arap_djzb 
( 
bbje, djbh, djdl, djkjnd, djkjqj, djlxbm, djrq, djzt, dr, dwbm, effectdate, fbje, fj, hzbz, lrr, lybz, prepay
, pzglh, qcbz, scomment, sxbz, vouchid	, xslxbm, ybje, ywbm, zyx10, zyx11, zyx12, zyx17, zyx4, zyx6, zyx7
, zyx8, zyx9, zzzt, zgyf, isselectedpay, isnetready, isjszxzf, isreded ,ts
) 
VALUES ( 

{taxTotalStr}				-- bbje 本币金额
, '{yfDanJuHao}'			-- djbh 单据编号  C-YF2101210005
, 'yf'						-- djdl 单据大类 yf（fk 采购付款单，yf 采购应付单，表arap_djlx）
, '{date:yyyy}'				-- djkjnd 单据会计年度 2021
, '{date:MM}'				-- djkjqj 单据会计期间 单据月 01
, 'D1'						-- djlxbm 单据类型编码 D1 采购应付单（D3 采购付款单，表arap_djlx）
, '{date:yyyy-MM-dd}'		-- djrq 单据日期 2021-01-15
, 1							-- djzt 单据状态 1已保存(未审核)  2已审核
, 0							-- dr 删除标志 0
, '{dwbm}'					-- dwbm 单位编码 1107 => 采云 [组织架构表.UniqueCode]
, '{date:yyyy-MM-dd}'		-- effectdate 起效日起 2021-01-15， 和辅表一致
, NULL						-- fbje 辅币金额 NULL
, {fjNum}					-- fj 附件数量
, -1						-- hzbz 坏帐标志 -1
, '{creator}'	            -- lrr 录入人 000166100000001HGVJC => 采云  配置：NCOption.DZCreator
, 4							-- lybz 来源标志 4（合同20，订单4）
, 'N'						-- prepay 预收付款标志 N
, 1							-- pzglh 系统标志 1
, 'N'						-- qcbz 期初标志 N
, '{otherRemark}'			-- scomment 其他_备注
, 0						    -- sxbz 生效标志 0未生效  / 10 已生效
, '{yf_vouchid}'	        -- vouchid 单据主表ID  C-11076610000000009NQH
, '0001O110000000001GTR'	-- xslxbm 销售类型编码  0001O110000000001GTE 一般采购业务员；0001O110000000001GTR 采购补录业务
, {taxTotalStr}				-- ybje 原币金额 550
, '{ywbm_yf}'	            -- ywbm 单据类型 select djlxoid from arap_djlx where djdl='yf' and DJLXBM='D1' and DWBM='[公司账套编码]' and dr = 0; （单据类型表arap_djlx）
, '{address}'	            -- zyx10 提单地 0001O1100000000FMC20 = 深圳（同步NC）
, '{bxsy}'				    -- zyx11 报销事由  付款事由 （转义）
, {baoxiaoJinE:F2}			-- zyx12 100（报销金额）
, '{yafa}'	                -- zyx17 是否研发 0001O11000000002S8KN 否（ 0001O11000000002S8KM是）
, NULL                      -- zyx4 2021-01-31
, NULL	                    -- zyx6 发票类型 10%增值税专用发票
, {yifu}				    -- zyx7 150（前期已付款）
, {fjNum}					-- zyx8 附件数量 5
, NULL	                    -- zyx9 0001O11000000008F0B7 （申购部门）（NULL）
, 0							-- zzzt 支付状态 0未支付 1已支付
, 0							-- zgyf 暂估应付标志 0
, 1							-- isselectedpay 选择付款 isselectedpay 1
, 'N'						-- isnetready 是否已经补录 N
, 'N'						-- isjszxzf 是否结算中心支付 N
, 'N' 						-- isreded 是否红冲
,'{GetDateTimeString()}'	-- ts 时间戳  2021-01-21 13:54:43

);
");

            #endregion

            #region 应付单 收付款协议表

            for (int i = 0; i < goodsList.Count; i++)
            {
                var item = goodsList[i];

                sb.AppendLine(@$"
-- 单据收付款协议表
INSERT INTO arap_djfkxyb ( 
bbye, dfbbje, dffbje, dfshl, dfybje, dr, fb_oid, fbye, fkxyb_oid, jfbbje, jffbje
, jfshl, jfybje, lastzkl, shlye, vouchid, xydqr, ybye ,ts
) 
VALUES ( 
{item.TaxTotal * huilv:F2}						-- bbye 本币余额
, {item.TaxTotal * huilv:F2}						-- dfbbje 贷方本币金额
, 0						-- dffbje 贷方辅币金额
, {item.Qty}						-- dfshl 贷方数量
, {item.TaxTotal:F2}						-- dfybje 贷方原币金额
, 0						-- dr 
, '{item.YfItemBillNo}'	-- fb_oid 单据辅表ID
, 0						-- fbye 辅币余额
, '{item.ShouFuKuanXieYiNo}'	-- fkxyb_oid 付款协议表ID 主键
, 0					-- jfbbje 借方本币金额
, 0						-- jffbje 借方辅币金额
, 0						-- jfshl  借方数量
, 0						-- jfybje 借方原币金额
, 0						-- lastzkl 最后折扣率
, {item.Qty}						-- shlye 数量余额
, '{yf_vouchid}'	-- vouchid 主表ID
, '{date:yyyy-MM-dd}'				-- xydqr 信用到期日
, {item.TaxTotal:F2}						-- ybye 原币余额
, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}'
);
");
            }

            #endregion

            #region 应付单 附表表sql

            for (int i = 0; i < goodsList.Count; i++)
            {
                var item = goodsList[i];

                sb.AppendLine(@$"
-- 应付单 附表 （{i + 1}/{goodsList.Count}）
INSERT INTO arap_djfb 
( 
bbhl, bbye, bjdwhsdj, bjdwsl, bjdwwsdj, bzbm, chmc, cinventoryid, cksqsh, ddhh, ddlx, deptid, dfbbje, dfbbsj, dfbbwsje		
, dffbje, dffbsj, dfshl, dfybje, dfybsj, dfybwsje, dj, djbh, dr, dwbm, fb_oid, fbhl, fbye, flbh, fph, fx			
, hbbm, hsdj, jfbbje, jfbbsj, jffbje, jffbsj, jfshl, jfybje, jfybsj, jfybwsje, jobid, jsfsbm, kslb, old_flag		
, old_sys_flag, pausetransact, ph, shlye, sl, txlx_bbje, txlx_fbje, txlx_ybje, vouchid, wbfbbje		
, wbffbje, wbfybje, wldx, xgbh, xyzh, ybye, ywbm, ywybm, zy, qxrq, issfkxychanged, ckdid, ckdh, billdate
, isverifyfinished, verifyfinisheddate, occupationmny, djdl, djlxbm, pk_cont,ts
) 
VALUES 
( 

{huilv}						-- bbhl 本币汇率:本位币设置为人民币，其他币种兑换人民币的汇率  原币*汇率=本币
, {item.TaxTotal * huilv:F2}					-- bbye 本币余额 = 原币余额 * 汇率 = 原币金额
, 0 						-- bjdwhsdj 报价单位含税单价
, 0 						-- bjdwsl 报价单位数量
, 0							-- bjdwwsdj 报价单位无税单价
, '{bizhong}'	-- bzbm 币种编码：0001O110000000005XMM 港币； 00010000000000000001 人民币 =》采云 select value from datadiction where id=采云币种Id
, NULL                      -- chmc 废弃1
, '{item.GoodsNCCode}'        -- cinventoryid 存货基本档案pk 0001O110000000007GH7
, 'C63-{_last}.{i + 1}'           -- cksqsh 源头单据行id: 入库单 ic_general_b主键 
, 'CFP-{_last}.{i + 1}' 	-- ddhh 上层来源单据行id = 发票体（行）Id 
, 'CFP-{_last}' 	-- ddlx 上层来源单据id = 发票主键Id
, '{deptid}'    -- deptid  部门pk  部门.UniqueCode
, {item.TaxTotal * huilv:F2}					-- dfbbje  贷方本币金额 价税合计 原币金额 550
, {item.Tax * huilv:F2}					-- dfbbsj   贷方本币税金 原币税金 = 原币无税金额 * 10%税率 = 50, 然后 50 * 0.8835 
, {item.Total * huilv:F2} 					-- dfbbwsje 贷方本币无税金额 原币无税金额 500 * 0.8835
, 0                         -- dffbje  贷方辅币金额 0
, 0                         -- dffbsj  贷方辅币税金 0
, {item.Qty:F2}                         -- dfshl 贷方数量
, {item.TaxTotal:F2}						-- dfybje 贷方原币金额 550
, {item.Tax:F2}						-- dfybsj 贷方原币税金
, {item.Total:F2}						-- dfybwsje 贷方原币无税金额 500
, {item.Price:F2}						-- dj 单价，不含税单价
, 'C-YF{date:yyMMdd}{randNo}'			-- djbh 单据编号冗余：主表DJBH  ？？
, 0							-- dr 删除标志 0
, '{dwbm}'					-- dwbm 公司pk 1107 => 采云 组织架构表.UniqueCode
, '{item.YfItemBillNo}'	-- fb_oid 辅表主键
, 0							-- fbhl 辅币汇率 0
, 0							-- fbye 辅币余额 0
, {i}							-- flbh 单据分录编号（从0开始  0  1  2  3）
, 'C-FP{_last}'	-- fph 发票号,发票维护单据号
, -1							-- fx 方向 0
, '{hbbm}'	-- hbbm 伙伴编码，客商编码 => 采云 供应商.SupplierNCPK
, {item.TaxPrice:F2}						-- hsdj 含税单价
, 0							-- jfbbje 借方本币金额 0
, 0							-- jfbbsj 借方本币税金 0
, 0							--  jffbje		 借方辅币金额 0
, 0							--  jffbsj		 借方辅币税金 0
, 0							--  jfshl			 借方数量 0
, 0							--  jfybje		 借方原币金额 0
, 0							--  jfybsj		 借方原币税金 0
, 0							--  jfybwsje		 借方原币无税金额 0
, {jobid}						--  jobid			 专项基本档案id NULL
, '25'						--  jsfsbm		 上层来源单据类型：25（21：采购订单  23：采购到货单  45：库存采购入库单  25：采购发票）
, {kslb}							--  kslb			 扣税类别   1：应税外加  （TODO：//0：应税内含 2：不计税）
, 'N'						--  old_flag		 无用10 N
, 'N'						--  old_sys_flag	 无用1 N
, 'N'						--  pausetransact	 挂起标志 N
, '45'						--  ph			 源头单据类型:45（21：采购订单  23：采购到货单  45：库存采购入库单  25：采购发票）
-- , '0001O110000000008L4S'	--  sfkxyh		 收付款协议号  => 采云 SupplierPaymentWay表.NCPK
, {item.Qty:F2}							--  shlye			 数量余额 
, {shuilv}						--  sl			 税率 10 代表 10%
, 0							--  txlx_bbje		 贴现利息本币金额 0
, 0							--  txlx_fbje		 贴现利息辅币金额 0
, 0							--  txlx_ybje		 贴现利息原币金额 0
, '{yf_vouchid}'	--  vouchid		 主表ID 
, 0							--  wbfbbje		 借方本币无税金额 0
, 0							--  wbffbje		 贷方辅币无税金额 0
, 0							--  wbfybje		 借方辅币无税金额 0
, 1							--  wldx			 往来对象标志 1
, -1						--  xgbh			 xgbh 并账标志 -1
, 'C63-{_last}'			--  xyzh			 源头单据id :入库单Id
, {item.TaxTotal:F2}						--  ybye			 原币余额
, '{ywbm_yf}'	--  ywbm 主表销售类型冗余（arap_djlx单据类型表主键）
, '{ywybm}'	--  ywybm			 业务员pk select * from bd_psndoc where pk_psndoc  ='000166100000001HTLUV' => 采云 EmployeeLatestInfo.UniqueCode
, '{zy}'			-- 摘要 SHL 华强方特（深圳）互联科技有限公司 | WHHQZN-024 1号仓库 | 芜湖市鸠江区赤铸山东路华强文化科技产业园动漫楼芜湖智能工厂 | 谢嗣峰 | 17775298975
, '{date:yyyy-MM-dd}'				--  qxrq			起效日期，手动录入，默认当天
, 'N'						-- issfkxychanged 收付款协议是否发生变化 N
, 'C63-{_last}.{i + 1}'			-- ckdid 出库单行id
, 'C63-{_last}'			-- ckdh 出库单号
, '{date:yyyy-MM-dd}'				-- billdate 单据日期
, 'N'						-- isverifyfinished 是否核销完成 N
, '3000-12-31'				-- verifyfinisheddate 核销完成日期 默认 3000-01-01
, 0							-- occupationmny 预占用核销原币余额 0
, 'yf'						-- djdl 单据大类 yf  （fk 采购付款单，yf 采购应付单，表arap_djlx）
, 'D1'						-- djlxbm 单据类型编码 D1应付单  D3付款单
, NULL 						-- pk_cont（无解释） NULL
,'{DateTime.Now:yyyy-MM-dd HH:mm:ss}'
);

");
            }
            #endregion

            //应付单审核
            sb.AppendLine(NcStep.YFApproval(input));

            sb.AppendLine("-- ===============================================================================================================");

            #region 付款单 主表sql

            sb.AppendLine($@"
-- 付款单 主表
INSERT INTO arap_djzb ( 
bbje, djbh, djdl, djkjnd, djkjqj, djlxbm, djrq, djzt, dr, dwbm, effectdate, fbje, fj, hzbz, lrr
, lybz, prepay, pzglh, qcbz, qrr, scomment, sxbz, vouchid, xslxbm, ybje, ywbm
, zyx10, zyx11, zyx12, zyx13, zyx15, zyx16, zyx17, zyx19, zyx20, zyx3, zyx4, zyx5, zyx6, zyx7, zyx8
, zyx9, zzzt, zgyf, isselectedpay, zyx30, isnetready, isjszxzf, isreded ,ts
) 
VALUES ( 
{taxTotal * huilv:F2}						-- bbhl 本币金额 本次付款应付金额 保留2位小数
, '{fkDanJuHao}'			-- djbh 单据编号 =>自动生成 C-FK21012000004
, 'fk'						-- djdl 单据大类 fk（fk 采购付款单，yf 采购应付单，表arap_djlx）
, '{date:yyyy}'					-- djkjnd 单据年
, '{date:MM}'						-- djkjqj 单据月
, 'D3'						-- djlxbm D3 采购付款单
, '{date:yyyy-MM-dd}'				-- djrq 单据日期
, 1						-- djzt 单据状态  1已保存(未审核)  2已审核  3已签字
, 0							-- dr 删除标志
, '{dwbm}'					-- dwbm 公司主键1107
, '{date:yyyy-MM-dd}'				-- effectdate 起效日起 2021-01-15
, {taxTotal:F2}						-- fbje 原币金额 原应付单总的应付原币金额
, {fjNum}							-- fj 附件数量
, -1						-- hzbz 坏帐标志
, '{creator}'	-- lrr 录入人 000166100000001HGVJC => 采云  配置：NCOption.DZCreator
, 1							-- lybz 来源标志 4（合同20，订单4，应付单1）
, 'N'						-- prepay 预收付款标志
, 1							-- pzglh 系统标志
, 'N'						-- qcbz 期初标志
, '{creator}'	-- qrr 确认人 000166100000001HGVJC => 采云  配置：NCOption.DZCreator
, '{otherRemark}'						-- scomment 其他-备注
, 0						-- sxbz 生效标志 0无效  10生效
, '{fk_vouchid}'	-- vouchid 主键 20位
, '0001O110000000001GTR'	-- xslxbm 销售类型编码  0001O110000000001GTE 一般采购业务员；0001O110000000001GTR 采购补录业务  
, {taxTotalStr}						-- ybje 原币金额
, '{ywbm_yf}'	-- ywbm 单据类型 000166100000001HTOCZ=采购应付单；  000166100000001HTLYA=采购付款单（单据类型表arap_djlx）
, '{address}'	-- zyx10 自定义项10 提单地  0001O1100000000FMC20深圳 0001O1100000000FMC21芜湖
, '{bxsy}'		-- zyx11 备注 报销事由
, {hetongjine:F2}					-- zyx12 合同金额 手工录入
, {yibaoxiao}							-- zyx13 扣款金额 一般为0
, '{deptid}'	-- zyx15 0001O11000000008F0B7（申购部门）（NULL）
, {yibaoxiao}						-- zyx16 前期已报销
, '{yafa}'	-- zyx17  0001O11000000002S8KN 否（是否研发 0001O11000000002S8KM是）
, '{contractno}'					-- zyx19 输入合同号
, {shifujine:F2}					-- zyx20 实付金额
, {hetongjine}					-- zyx3 结算金额 = 合同金额
, '{endDate:yyyy-MM-dd}'				-- zyx4 月底？
, '项目名称'				-- zyx5 录入项目名称
, '{shuilv}%增值税专用发票'		-- zyx6 发票类型
, {yifu}						-- zyx7 150（前期已付款）
, {fjNum}						-- zyx8 附件数量 5
, '{F0B7}'	-- zyx9 0001O11000000008F0B7 （申购部门）（NULL）
, 0							-- zzzt 支付状态 0未支付  1已支付
, 0							-- zgyf 暂估应付标志 
, 1							-- isselectedpay 选择付款 1
, '{fukuanxieyi}'	-- zyx30 0001O110000000008L57（关联付款协议表bd_payterm主键）=> 采云 SupplierPaymentWay表.NCPK
, 'N'						-- isnetready 是否已经补录 
, 'N'						-- isjszxzf 是否结算中心支付
, 'N'						-- isreded 是否红冲
, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}'
);
");

            #endregion

            #region 付款单附表 sql

            for (int i = 0; i < goodsList.Count; i++)
            {
                var item = goodsList[i];

                sb.AppendLine($@"
-- 付款单 附表 （{i + 1}/{goodsList.Count}）
INSERT INTO arap_djfb ( 
bbhl, bbye, bjdwhsdj, bjdwsl, bjdwwsdj, bzbm, chmc, cinventoryid, ddhh, ddlx, deptid
, dfbbje, dfbbsj, dfbbwsje, dffbje, dffbsj, dfshl, dfybje, dfybsj, dfybwsje, dj, djbh, dr, dwbm
, fb_oid, fbhl, fbye, fkyhmc, flbh, fph, fx, hbbm, hsdj, jfbbje, jfbbsj, jffbje, jffbsj, jfshl
, jfybje, jfybsj, jfybwsje, jobid, jsfsbm, kslb, old_sys_flag, pausetransact, shlye, sl, txlx_bbje
, txlx_fbje, txlx_ybje, vouchid, wbfbbje, wbffbje, wbfybje, wldx, xgbh, ybye, ywbm, ywybm, zjldw
, zy, qxrq, issfkxychanged, payflag, billdate, isverifyfinished, verifyfinisheddate, djxtflag
, dfyhzh, occupationmny, djdl, djlxbm ,ts

) 
VALUES ( 

{huilv}						-- bbhl 本币汇率
, {item.TaxTotal * huilv:F2}	-- bbye 本币余额
, 0							-- bjdwhsdj 报价单位含税单价
, 0							-- bjdwsl 报价单位数量
, 0							-- bjdwwsdj 报价单位无税单价
, '{bizhong}'	-- bzbm 币种编码
, NULL						-- chmc 废弃1
, '{item.GoodsNCCode}'	-- cinventoryid 存货基本档案pk 物料主键 => 物料.[NCCode]   select pk_invbasdoc from bd_invbasdoc where invcode = 'WH1006020820165' -- 根据NCCode获取
, '{item.YfItemBillNo}'	-- ddhh 上层来源单据行id 应付单行号 11076610000000009NQI
, '{yf_vouchid}'	-- ddlx 上层来源单据id 应付单 主键
, '{deptid}'	-- deptid 部门pk => 部门.UniqueCode (NULL)
, 0							-- dfbbje 贷方本币金额
, 0							-- dfbbsj 贷方本币税金
, 0							-- dfbbwsje 贷方本币无税金额
, 0							-- dffbje 贷方辅币金额
, 0							-- dffbsj  贷方辅币税金
, 0							-- dfshl 贷方数量
, 0							-- dfybje  贷方原币金额
, 0							-- dfybsj  贷方原币税金
, 0							-- dfybwsje  贷方原币无税金额
, {item.Price:F8}			-- dj 单价 无税单价
, '{fkDanJuHao}'			-- djbh 单据编号冗余 付款单号
, 0							-- dr 删除标志
, '{dwbm}'					-- dwbm 公司pk => 组织架构表.UniqueCode
, '{item.FkItemBillNo}'	-- fb_oid 主键 附表主键
, 0							-- fbhl 辅币汇率
, 0							-- fbye 辅币余额
, '{bankName}'		-- fkyhmc 付款银行名称
, {i}							-- flbh 单据分录编号 类似行号0,1,2,3
, '{fph}'		-- fph 发票维护单据号 My_FaPiaoWeiHu_003
, 1							-- fx 方向 1 （应付单0  付款单1）
, '{hbbm}'	-- hbbm 伙伴编码 0001O11000000009FZVJ 供应商编码 => 供应商.SupplierNCPK
, {item.TaxPrice:F8}						-- hsdj 含税单价
, {item.TaxTotal * huilv:F2}					-- jfbbje 借方本币金额
, {item.Tax:F2}						-- jfbbsj 借方本币税金
, 0							-- jffbje 借方辅币金额
, 0							-- jffbsj 借方辅币税金
, {item.Qty}							-- jfshl 借方数量
, {item.TaxTotal:F2}						-- jfybje 借方原币金额
, {item.Tax:F2}						-- jfybsj 借方原币税金
, {item.Total:F2}						-- jfybwsje 借方原币无税金额
, '{jobid}'	-- jobid 专项基本档案id (NULL)
, 'D1'						-- jsfsbm 上层来源单据类型 
, {kslb}							-- kslb 扣税类别 
, 'N'						-- old_sys_flag  无用1
, 'N'						-- pausetransact  挂起标志
, {item.Qty}				-- shlye  数量余额
, {shuilv}						-- sl 税率 10%
, 0							-- txlx_bbje 贴现利息本币金额
, 0							-- txlx_fbje 贴现利息辅币金额
, 0							-- txlx_ybje 贴现利息原币金额
, '{fk_vouchid}'	-- vouchid 主表ID 
, {item.Total * huilv:F2}					-- wbfbbje 借方本币无税金额
, 0							-- wbffbje 贷方辅币无税金额
, 0							-- wbfybje 借方辅币无税金额
, 1							-- wldx 往来对象标志
, -1						-- xgbh 并账标志
, {item.TaxTotal:F2}		-- ybye 原币余额
, '{ywbm_fk}'	-- ywbm 单据类型pk冗余 000166100000001HTOCZ=采购应付单；  000166100000001HTLYA=采购付款单（单据类型表arap_djlx）
, '{ywybm}'	-- ywybm 业务员pk 000166100000001HTLUV
, '0001O110000000000MT3'	-- zjldw 废弃3  NULL
, '{zy}'	-- zy 摘要 
, '{date:yyyy-MM-dd}'				-- qxrq 起效日期
, 'N'						-- issfkxychanged  收付款协议是否发生变化
, NULL						-- payflag 支付状态 0未支付 1已支付
, '{date:yyyy-MM-dd}'				-- billdate  单据日期
, 'N'						-- isverifyfinished 是否核销完成
, '3000-01-01'				-- verifyfinisheddate 核销完成日期
, 0							-- djxtflag 单据协同标志 
, '{bankNumber}'	-- dfyhzh 对方银行帐号 (bd_bankaccbas银行账户基本信息表主键)
, {item.TaxTotal:F2}		-- occupationmny 预占用核销原币余额
, 'fk'						-- djdl 单据大类 fk
, 'D3'						-- djlxbm 单据类型编码 
, '{GetDateTimeString()}'		-- ts 时间戳

);
");
            }

            #endregion

            #region 更新应付单

            for (int i = 0; i < goodsList.Count; i++)
            {
                var item = goodsList[i];

                sb.AppendLine($@"
-- 回写 应付单 附表 预占用核销原币余额 （{i + 1}/{goodsList.Count}）
update arap_djfb set ts='{DateTime.Now:yyyy-MM-dd HH:mm:ss}',occupationmny = occupationmny + {item.TaxTotal:F2}  where fb_oid = '{item.YfItemBillNo}';

");
            }

            #endregion

            #region 核销关系对照表  arap_billmap

            for (int i = 0; i < goodsList.Count; i++)
            {
                var item = goodsList[i];

                sb.AppendLine($@"
-- 核销关系对照表 arap_billmap
INSERT INTO arap_billmap ( 
arapflag, s_system, ybje, t_billtype, t_billid, oldje, ybye, pk_billmap
, s_billtype, t_class, holdflag, t_itemid, s_itemid, s_billid, dr ,ts
) 
VALUES (
1							-- arapflag 收付标志 1付款
, 1							-- s_system 系统来源
, {item.TaxTotal:F2}						-- ybje 付款金额
, 'D3'						-- t_billtype 目标单据类型：付款单
, '{fk_vouchid}'	-- t_billid  目标单据pk：付款单主键
, {item.TaxTotal:F2}						-- oldje 历史余额
, {item.TaxTotal:F2}						-- ybye 付款余额
, 'C-HX{_last}.{i + 1}'	-- pk_billmap 主键
, 'D1'						-- s_billtype 源单据类型：应付单
, 'fk'						-- t_class 目标单据大类 付款
, 'Y'						-- holdflag 占用标志 
, '{item.FkItemBillNo}'	-- t_itemid 目标单据行pk：付款单行Id
, '{item.YfItemBillNo}'	-- s_itemid  源单据行pk：应付单行Id
, '{yf_vouchid}'	-- s_billid 源单据pk ：应付单id
, 0							-- dr 删除标志
, '{GetDateTimeString()}'
 );

");
            }

            #endregion

            #region 付款单 单据收付款协议表

            for (int i = 0; i < goodsList.Count; i++)
            {
                var item = goodsList[i];

                sb.AppendLine($@"
-- 单据收付款协议表
INSERT INTO arap_djfkxyb ( 
bbye, dfbbje, dffbje, dfshl, dfybje, dr, fb_oid, fbye, fkxyb_oid, jfbbje, jffbje
, jfshl, jfybje, lastzkl, shlye, vouchid, xydqr, ybye ,ts
) 
VALUES ( 
{item.TaxTotal * huilv:F2}						-- bbye 本币余额
, '0'						-- dfbbje 贷方本币金额
, '0'						-- dffbje 贷方辅币金额
, '0'						-- dfshl 贷方数量
, '0'						-- dfybje 贷方原币金额
, '0'						-- dr 
, '{item.FkItemBillNo}'	-- fb_oid 单据辅表ID  付款单行Id
, '0'						-- fbye 辅币余额
, 'C-XYF{_last}.{i + 1}'	-- fkxyb_oid 主键
, {item.TaxTotal * huilv:F2}					-- jfbbje 借方本币金额
, '0'						-- jffbje 借方辅币金额
, {item.Qty:F2}						-- jfshl  借方数量
, {item.TaxTotal:F2}						-- jfybje 借方原币金额
, '0'						-- lastzkl 最后折扣率
, {item.Qty}						-- shlye 数量余额
, '{fk_vouchid}'	-- vouchid 主表ID 付款单Id
, '{dateStr}'				-- xydqr 信用到期日
, {item.TaxTotal:F2}						-- ybye 原币余额
, '{GetDateTimeString()}'
);

");
            }

            #endregion

            #region 结算信息

            //结算主表
            sb.AppendLine($@"
-- 插入结算信息
INSERT INTO cmp_settlement ( 
iswagelisting, isbusicontrol, pk_finorg, busistatus, settlestatus, arithmetic, effectstatus, busi_billdate, pk_settlement
, isbusieffect, pk_operator, systemcode, pk_tradetype, pk_ftsbill, pk_executor, lastupdatedate
, iscmcandeal, busi_auditdate, settletype, pk_busibill
, def20, def4, pk_signer, def3, def2, def1, def12, def13, def10
, pk_auditor, def11, isautosign, isautopay, direction
, def18, def19, def16, def17, def14, def15
, isautogenerate, pk_busitype, dr, isapplybill, pk_corp, billcode, payway, signdate, issettleeffect, setlledate
, fts_billtype, setlleno, primal, pk_billoperator, ispay, local, aduitstatus, lastupdater ,ts
) 
VALUES (
NULL        -- iswagelisting 是否携带工资清单
, NULL      -- isbusicontrol 是否业务控制数据处理
, NULL      -- pk_finorg 财务组织（预留）
, 2       -- busistatus 业务单据状态 2未审核 / 4已审核 Update
, '0'       -- settlestatus 结算状态 0
, '0'       -- arithmetic 结算信息处理算法  
, 0       -- effectstatus 业务单据生效状态  0未生效 / 10已生效 Update
, '{dateStr}'      -- busi_billdate 业务单据日期
, '{settlementKey}'        -- pk_settlement 结算信息 主键
, 'N'       -- isbusieffect 业务单据是否已生效 （是否已审核签字）N / Y Update
, '{creator}'        -- pk_operator 录入人
, 'arap'        -- systemcode 归属系统
, 'D3'          -- pk_tradetype 业务单单据类型
, NULL          -- pk_ftsbill 委托收付款单主键
, NULL          -- pk_executor 结算人
, '{dateStr}'  -- lastupdatedate 最新更新日期
, 'Y'           -- iscmcandeal 是否结算平台可处理
, NULL          -- busi_auditdate  业务单据审核日期 NULL / 2021-01-08 Update
, '4'           -- settletype 完成结算方式
, '{fk_vouchid}'        -- pk_busibill 业务单据主键
, NULL, NULL
, NULL          -- pk_signer 签字确认人 NULL / 000166100000001HGVJC Update
, NULL, NULL, NULL, NULL, NULL, NULL
, NULL          -- pk_auditor 业务单据审核人 NULL / 000166100000001HGVJC Update
, NULL
, 'Y'           -- isautosign 是否自动签字
, NULL          -- isautopay 是否自动支付
, '1'           -- direction 方向
, NULL, NULL, NULL, NULL, NULL, NULL
, NULL          -- isautogenerate 是否自动生成
, '0001O110000000001GTR'        -- pk_busitype 单据业务类型 0001O110000000001GTE 一般采购业务员；0001O110000000001GTR 采购补录业务
, '0'           -- dr 删除标识
, 'N'           -- isapplybill 收付申请单
, '{dwbm}'        -- pk_corp 公司pk
, '{fkDanJuHao}'        -- billcode 业务单据编号 付款单单据号
, NULL          -- payway int 收付款方式
, NULL          -- signdate 签字确认日期 NULL / 2012-01-08 Update
, 'N'           -- issettleeffect  结算单是否生效
, NULL          -- setlledate 结算日期
, NULL          -- fts_billtype 委托收付款单单据类型
, NULL          -- setlleno 结算号
, {taxTotal:F2}        -- primal 原币金额
, '{creator}'        -- pk_billoperator  业务单据录入人 000166100000001HGVJC
, 'Y'           -- ispay 是否委付
, {taxTotal * huilv:F2}        -- local 本币金额
, '0'           -- aduitstatus 业务单据审批状态 4 / 0 Update
, '{creator}'        -- lastupdater 最新更新人 000166100000001HGVJC
, '{GetDateTimeString()}'         -- ts 时间戳
);

");

            for (int i = 0; i < goodsList.Count; i++)
            {
                var item = goodsList[i];
                var itemKey = $"{input.Dwbm}6610000000{input.RandNo + i}AE";//100966100000000SLAV4

                //结算明细
                sb.AppendLine($@"
-- 结算信息明细 
INSERT INTO cmp_detail 
( 
settlelineno, lastest_paydate, isacccord, pk_finorg, oppbank, pk_notetype, pk_account, processtype, expectate_date, issamebank
, def7, def8, def5, def6, def9, systemcode, tallystatus, oppaccount, checkcount, def4, fundformcode, def3, def2, def1, def12
, pk_psndoc, def13, def10, def11, memo, paylocal, def18, def19, def16, def17, def14, def15, pk_balatype, billtime, checkdate
, fracrate, pk_busitype, pk_oppbank, pk_innercorp, bankrelated_code, pk_trader, dr, transtype, pk_billdetail, tradertype, signdate
, pk_cashflow, tradername, pk_job, pk_deptdoc, busilineno, exectime, tallydate, pk_paybill, settlestatus, pk_rescenter, execdate
, pk_currtype, pay_type, signtime, pk_settlement, pk_bill, pk_billtype, receivelocal, payfrac, code, localrate, modifyflag, pk_jobphase
, paybillcode, fundsflag, def20, bankmsg, notenumber, pk_detail, pk_invcl, pk_plansubj, pk_cubasdoc, notedirection, pk_assaccount
, direction, pk_bank, groupno, pk_oppaccount, pk_invbasdoc, billdate, pay, receivefrac, receive, pk_costsubj, pk_corp, billcode
, issamecity, pk_billbalatype, pk_mngaccount, isbillrecord ,ts
) 
VALUES ( 

{i}                 -- settlelineno 结算单行号
, NULL              -- lastest_paydate 最迟支付日期
, 'N'               -- isacccord 是否记账记录
, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
, 'arap'            -- systemcode 来源系统
, 2                 -- tallystatus 记账状态 1 记账后为 2 Update
, NULL              -- oppaccount 对方银行账号 
, 0                 -- checkcount 对帐次数
, NULL, 3           -- fundformcode 资金形态 
, NULL, NULL, NULL, NULL
, '1009A8100000000001DK'                    -- pk_psndoc 人员档案 1009A8100000000001DK
, NULL, NULL, NULL, '物料备注'         -- memo 备注
, {item.TaxTotal:F2}               -- paylocal 付款本币 
, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
, '{dateStr}'                      -- checkdate 对账日期
, NULL, '0001O110000000001GTR'      -- pk_busitype 业务类型 0001O110000000001GTE 一般采购业务员；0001O110000000001GTR 采购补录业务
, '{bankNumber}'                    -- pk_oppbank 对方银行主键 0001O11000000003GV0N
, NULL, NULL
, '0001O1100000000209N1'            -- pk_trader 对方 0001O1100000000209N1 ？？
, '0'                               -- dr 删除标志
, '1'                               -- transtype 交易种类
, '{item.FkItemBillNo}'             -- pk_billdetail 来源单据行id 付款单附表主键 Iid
, '0'                               -- tradertype  对方类型
, NULL                              -- signdate 签字确认日期 NULL / 2021-01-08 Update
, NULL                              -- pk_cashflow 现金流量项目
, NULL                              -- tradername 对方对象名称
, NULL                              -- pk_job 项目 NULL  0001O11000000002KEVV
, NULL                              -- pk_deptdoc 部门 指采购部门 1009O1100000000GCYO1 select* from bd_deptdoc where pk_deptdoc='1009O1100000000GCYO1'
, {i}                               -- busilineno  业务单据行号，付款单单据行号
, NULL, NULL, NULL, NULL, NULL, NULL,
'{bizhong}'                         -- pk_currtype 币种
, NULL, NULL
, '{settlementKey}'            -- pk_settlement 结算信息 主键 100966100000000SLAV3
, '{input.FKVouchid}'           -- pk_bill 来源单据id 付款单主键 100966100000000SLAUW
, 'D3'                              -- pk_billtype 来源单据类型 D3 付款单
, '0'                               -- receivelocal 收款本币 
, '0'                               -- payfrac 付款辅币
, NULL                              -- code 数字签名 
, {input.HuiLv}                     -- localrate 本币汇率
, 'n,n,n,n,n,n,n,n,n,n,n,n,n'       -- modifyflag 手工修改标志
, NULL, NULL
, 1                                 -- fundsflag 资金流向
, NULL, NULL, NULL
, '{itemKey}'                       -- pk_detail 结算明细 主键 
, NULL                              -- pk_invcl 存货分类 0001O11000000000472Z
, NULL
, '{hbbm}'                          -- pk_cubasdoc 客商档案 供应商
, '0'                               -- notedirection 票据方向
, NULL
, '1'                               -- direction 方向
, NULL, NULL
, '{gongYingShangBankKey}'          -- pk_oppaccount 对方帐户主键 ?? 0001O110000000020CXC
, '{item.GoodsNCCode}'              -- pk_invbasdoc 存货档案
, '{input.BillDate:yyyy-MM-dd}'     -- billdate 单据日期
, {item.TaxTotal:F2}                -- pay 付款原币
, 0                                 -- receivefrac  收款辅币
, 0                                 -- receive 收款原币
, NULL
, '{input.Dwbm}'                    -- pk_corp 公司编码
, '{input.FKBillNo}'                -- billcode 单据编号 付款单单据号
, NULL, NULL, NULL, NULL
, '{GetDateTimeString()}'

);

");
            }



            #endregion

            #region 付款单审核 签字

            //            sb.AppendLine($@"
            //-- 付款单审核
            //update arap_djzb set ts = '{DateTime.Now:yyyy-MM-dd HH:mm:ss}'
            //, sxbz = 10                     -- sxbz 生效标志
            //, sxr = '{creator}'  -- sxr 生效人 000166100000001HGVJC
            //, sxkjnd = '{date:yyyy}'               -- sxkjnd 生效年度 2021
            //, sxkjqj = '{date:MM}'                 -- sxkjqj 生效期间 月 01
            //, sxrq = '{date:yyyy-MM-dd}'           -- sxrq 生效日期   2021-01-15

            //,spzt = '1'                            -- spzt 审批状态
            //,djzt = 2                              -- djzt 单据状态
            //,shr = '{input.Creator}'
            //,shkjnd = '{input.BillDate.Year}', shkjqj = '{input.BillDate:MM}', shrq = '{input.BillDate:yyyy-MM-dd}'

            //where vouchid = '{fk_vouchid}';
            //");

            #endregion

            textBox1.Text = sb.ToString();
        }

        private static List<GoodsCost> GetGoodsCost(InputDto input)
        {
            decimal noTaxMoney = input.NoTaxMoney;//, decimal taxRate, DateTime date, int randNo

            //固定三种物料
            var goodsNCCodes = new string[] { "0001O110000000006PTV", "0001O1100000000ESQJM", "0001O110000000006P9N" };

            var list = new List<GoodsCost>();
            var sum = 0M;
            for (var i = 0; i < goodsNCCodes.Length; i++)
            {
                //数量
                var qty = (decimal)(i + 1) * 3; //随机数量
                // 原币无税金额
                var total = noTaxMoney / 12 * (i + 3); // 金额按345等分
                total = Math.Round(total, 2, MidpointRounding.AwayFromZero);

                if (i == goodsNCCodes.Length - 1)
                {
                    total = noTaxMoney - sum;
                }
                sum += total;

                var item = new GoodsCost(goodsNCCodes[i], input.TaxRate, qty, total);
                item.YfItemBillNo = $"{input.Dwbm}6610000000{input.RandNo + i}CF";
                item.FkItemBillNo = $"{input.Dwbm}6610000000{input.RandNo + i}BF";
                item.ShouFuKuanXieYiNo = $"{input.Dwbm}6610000000{input.RandNo + i}DF"; //11076610000000009NV3
                list.Add(item);

            }

            return list;

        }

        private static string GetDateTimeString()
        {
            return CommonHelper.GetDateTimeString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GoodsCost
    {
        public GoodsCost(string ncCode, decimal taxRate, decimal qty, decimal total)
        {
            GoodsNCCode = ncCode;
            TaxRate = taxRate;
            Qty = qty;
            Total = total;
        }

        /// <summary>
        /// 应付单子表主键
        /// </summary>
        public string YfItemBillNo { get; set; }

        /// <summary>
        /// 付款单子表主键
        /// </summary>
        public string FkItemBillNo { get; set; }

        /// <summary>
        /// 收付款协议主键
        /// </summary>
        public string ShouFuKuanXieYiNo { get; set; }

        /// <summary>
        /// 物料NCCode
        /// </summary>
        public string GoodsNCCode { get; }

        /// <summary>
        /// 税率 13
        /// </summary>
        public decimal TaxRate { get; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; }

        /// <summary>
        /// 无税总额
        /// </summary>
        public decimal Total { get; }





        /// <summary>
        /// 含税总额
        /// </summary>
        public decimal TaxTotal => Tax + Total;

        /// <summary>
        /// 无税单价
        /// </summary>
        public decimal Price => Total / Qty;

        /// <summary>
        /// 含税单价 (应税外加)
        /// </summary>
        public decimal TaxPrice => (TaxRate / 100M + 1M) * Price;

        /// <summary>
        /// 税额
        /// </summary>
        public decimal Tax => Price * TaxRate / 100M * Qty;

    }

}
