using System;

namespace KYH_ProductPrice.Models.PublicSqlMethods
{
	// Token: 0x02000015 RID: 21
	public class ResponseJson
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00003553 File Offset: 0x00001753
		// (set) Token: 0x06000133 RID: 307 RVA: 0x0000355B File Offset: 0x0000175B
		public int id { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00003564 File Offset: 0x00001764
		// (set) Token: 0x06000135 RID: 309 RVA: 0x0000356C File Offset: 0x0000176C
		public int Success { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00003575 File Offset: 0x00001775
		// (set) Token: 0x06000137 RID: 311 RVA: 0x0000357D File Offset: 0x0000177D
		public string Msg { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00003586 File Offset: 0x00001786
		// (set) Token: 0x06000139 RID: 313 RVA: 0x0000358E File Offset: 0x0000178E
		public object Data { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00003597 File Offset: 0x00001797
		// (set) Token: 0x0600013B RID: 315 RVA: 0x0000359F File Offset: 0x0000179F
		public int count { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600013C RID: 316 RVA: 0x000035A8 File Offset: 0x000017A8
		// (set) Token: 0x0600013D RID: 317 RVA: 0x000035B0 File Offset: 0x000017B0
		public int pape { get; set; }
	}
}
