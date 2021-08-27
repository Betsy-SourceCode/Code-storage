using System;

namespace KYH_Customer.Models.ViewDataModel
{
	// Token: 0x0200001D RID: 29
	public class AuditInfo
	{
		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x00005606 File Offset: 0x00003806
		// (set) Token: 0x060005F1 RID: 1521 RVA: 0x0000560E File Offset: 0x0000380E
		public int CustomerId { get; set; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x00005617 File Offset: 0x00003817
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x0000561F File Offset: 0x0000381F
		public string SORcvEntity { get; set; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x00005628 File Offset: 0x00003828
		// (set) Token: 0x060005F5 RID: 1525 RVA: 0x00005630 File Offset: 0x00003830
		public string MFGProcEntity { get; set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x00005639 File Offset: 0x00003839
		// (set) Token: 0x060005F7 RID: 1527 RVA: 0x00005641 File Offset: 0x00003841
		public string FamilyTree { get; set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0000564A File Offset: 0x0000384A
		// (set) Token: 0x060005F9 RID: 1529 RVA: 0x00005652 File Offset: 0x00003852
		public string DocCheckList { get; set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x0000565B File Offset: 0x0000385B
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x00005663 File Offset: 0x00003863
		public string SCACOpinion { get; set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x0000566C File Offset: 0x0000386C
		// (set) Token: 0x060005FD RID: 1533 RVA: 0x00005674 File Offset: 0x00003874
		public string MDCreateBy { get; set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x0000567D File Offset: 0x0000387D
		// (set) Token: 0x060005FF RID: 1535 RVA: 0x00005685 File Offset: 0x00003885
		public string MDCreateDept { get; set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x0000568E File Offset: 0x0000388E
		// (set) Token: 0x06000601 RID: 1537 RVA: 0x00005696 File Offset: 0x00003896
		public DateTime? MDCreateTime { get; set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x0000569F File Offset: 0x0000389F
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x000056A7 File Offset: 0x000038A7
		public string CWACOpinion { get; set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x000056B0 File Offset: 0x000038B0
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x000056B8 File Offset: 0x000038B8
		public string ACCreateBy { get; set; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x000056C1 File Offset: 0x000038C1
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x000056C9 File Offset: 0x000038C9
		public string ACCreateDept { get; set; }

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x000056D2 File Offset: 0x000038D2
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x000056DA File Offset: 0x000038DA
		public DateTime? ACCreateTime { get; set; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x000056E3 File Offset: 0x000038E3
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x000056EB File Offset: 0x000038EB
		public string createBy { get; set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x000056F4 File Offset: 0x000038F4
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x000056FC File Offset: 0x000038FC
		public string createDept { get; set; }
	}
}
