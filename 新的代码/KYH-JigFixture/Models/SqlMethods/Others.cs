using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYH_JigFixture.Models.SqlMethods
{
	public class Others
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000034C2 File Offset: 0x000016C2
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x000034CA File Offset: 0x000016CA
		public int id { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000034D3 File Offset: 0x000016D3
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x000034DB File Offset: 0x000016DB
		public string Text { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000034E4 File Offset: 0x000016E4
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000034EC File Offset: 0x000016EC
		public string K3PO_Num { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000034F5 File Offset: 0x000016F5
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x000034FD File Offset: 0x000016FD
		public DateTime? K3PO_Date { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00003506 File Offset: 0x00001706
		// (set) Token: 0x060000EB RID: 235 RVA: 0x0000350E File Offset: 0x0000170E
		public string Supplier { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00003517 File Offset: 0x00001717
		// (set) Token: 0x060000ED RID: 237 RVA: 0x0000351F File Offset: 0x0000171F
		public string Buyer { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00003528 File Offset: 0x00001728
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00003530 File Offset: 0x00001730
		public string k3Unit { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00003539 File Offset: 0x00001739
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00003541 File Offset: 0x00001741
		public decimal? K3FQty { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000354A File Offset: 0x0000174A
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00003552 File Offset: 0x00001752
		public DateTime? Prop_Arr_Date { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000355B File Offset: 0x0000175B
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00003563 File Offset: 0x00001763
		public DateTime? K3Recv_Date { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x0000356C File Offset: 0x0000176C
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00003574 File Offset: 0x00001774
		public object Data { get; set; }
	}
}