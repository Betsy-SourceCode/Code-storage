using System;

namespace KYH_KnowledgeBase.Models.PublicSqlMethods
{
	// Token: 0x0200000B RID: 11
	public class PageJson
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002C07 File Offset: 0x00000E07
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002C0F File Offset: 0x00000E0F
		public int id { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002C18 File Offset: 0x00000E18
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002C20 File Offset: 0x00000E20
		public int success { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002C29 File Offset: 0x00000E29
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002C31 File Offset: 0x00000E31
		public int count { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002C3A File Offset: 0x00000E3A
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002C42 File Offset: 0x00000E42
		public int size { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002C4B File Offset: 0x00000E4B
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002C53 File Offset: 0x00000E53
		public string Msg { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002C5C File Offset: 0x00000E5C
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002C64 File Offset: 0x00000E64
		public object data { get; set; }
	}
}
