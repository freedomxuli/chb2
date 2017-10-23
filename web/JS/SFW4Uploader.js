Ext.define('SFW4.UploaderPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.UploaderPanel',
    constructor: function(option) {
        if (!option.fieldLabel)
            option.fieldLabel = '附件';
        if (!option.buttonText)
            option.buttonText = '选择';
//        option.items = [{
//            xtype: 'filefield',
//            fieldLabel: option.fieldLabel,
//            buttonText: option.buttonText
//}];
            if (!option.bodyPadding)
                option.bodyPadding = 10;
            if (!option.border)
                option.border = false;
            this.callParent([option]);
            this.upload = function(method, onSuccess, onFailure) {
                this.submit({
                    url: 'cse/' + method,
                    waitMsg: '上传中...',
                    params: { args: createParams(arguments) },
                    success: function(a, b) {
                        if (b.response.responseText != "") {
                            if (b.result.succeed == 1) {
                                onSuccess(b.result.retVal, method);
                            }
                            else {
                                onFailure(b.result.retVal, method);
                            }
                        }
                        else {
                            onFailure("连接失败或超出文件大小限制", method);
                        }
                    },
                    failure: function (a, b) {
                        if (b.result.retVal)
                            onFailure(b.result.retVal, method)
                        else
                            onFailure("连接失败或超出文件大小限制", method);
                    }
                });
            }
        }
    });