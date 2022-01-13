$(function () {
    tastySelect();
    jQuery(function ($) {
        $(".datatable").dataTable({
            "paging": false,
            "searching": false,
            "info": false,
            "orderCellsTop": true,
            "bAutowidth": false,  //固定表头
            aaSorting: [0, 'desc'], // 默认排序
            aoColumnDefs: [
                {
                    orderSequence: ["desc", "asc"],
                    "bSortable": false,
                    "aTargets": [2, 3, 4, 5, 6, 7, 8, 10]  // 哪些列不排序
                }
            ]
        });
    });
})
//通过账套和物料号带出物料名称规格。找不到这物料号的，物料名称规格显示“没有这物料！”
function GetMessage() {
    //判断长度是否正确
    if ($("#Fnumber").val().length != 18) {
        swal('操作失败!', '您输入的物料号格式不正确，正确物料号格式为：带小数点共18位，请重新输入', 'error') //提示框
        //清空
        $("#Fnumber").val('');
        $("#FModel").html('');
        return false;
    }
    $.ajax({
        url: "/InventoryTracking/InventoryTracking/GetMaterialMessage",
        data: { "Ledger": $("#tastySelect").val(), "Fnumber": $("#Fnumber").val() },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data != "error") {
                if (data != null) {
                    //显示名称规格
                    $("#FModel").html(data);
                }
                else {
                    swal('没有此物料!', '', 'error') //提示框
                    //清空
                    $("#Fnumber").val('');
                    $("#FModel").html('');
                }
            }
            else {
                swal('操作失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                //清空
                $("#Fnumber").val('');
                $("#FModel").val('');
            }
        }
    })
}
//正则限制只能输入数字和小数点
function clearNoNum(obj) {
    obj.value = obj.value.replace(/[^\d.]/g, "");//清除“数字”和“.”以外的字符
    obj.value = obj.value.replace(/^\./g, "");//验证第一个字符是数字而不是.
}