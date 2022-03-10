/**
 *新增需求：新增修改产品描述功能 
 * 显示功能按钮和富文本框
 */
function OpenProductDescription() {
    //特殊情况：先点开了修改备注再点修改产品描述图片，关闭修改备注的文本框
    if (QJRemarks != null) {
        $("#RemarksmOQ_label").css("display", "");
        $("#RemarksMD_label").css("display", "");
        $("#RemarksFD_label").css("display", "");
        $("#Remarks_mOQ").css("display", "none");
        $("#Remarks_MD").css("display", "none");
        $("#Remarks_FD").css("display", "none");
    }
    //修改页面标题
    $("#ChineseTitle").html("修 改 产 品 描 述");
    $("#EnglishTitle").html("Amend Product Description");
    //隐藏其他所有功能按钮，显示修改产品描述功能按钮
    $("#MainBtn").css("display", "none");
    $("#OtherBtn").css("display", "");
    //显示修改产品描述富文本框
    $("#OtherDiv").css("display", "grid");
}
/**
 *新增需求：新增修改产品描述功能
 * 修改产品描述按钮-确认
 */
function AmendProductDescription(CPSerial) {
    var CustProdName = $("#OtherText").val();//获取富文本的内容
    if (CustProdName == "") {
        alert("产品描述不能为空！");
        return false;
    }
    else {
        $.ajax({
            url: "/ProductPrice/ProductPriceZG/ChangeCustProductPriceRecords",
            data: { "CustProdName": CustProdName, "CPSerial": CPSerial, "num": 3, "type": 0 },
            type: "POST",
            dataType: "json",
            success: function (data) {
                if (data.Success > 0) {
                    alert("更改成功！");
                    location.reload(); //刷新页面
                }
                else {
                    alert("更改失败，发生错误，请联系电脑部！内部成员请查看日志文件！");
                }
            }
        })
    }

}
