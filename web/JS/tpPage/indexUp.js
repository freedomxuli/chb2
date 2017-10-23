
//查询地区数据源
var dqstore = Ext.create('Ext.data.Store', {
    fields: ['VALUE', 'TEXT'],
    data : [
    ]
});

//查询评价商店数据源
var pjsdstore = Ext.create('Ext.data.Store', {
    fields: ['VALUE', 'TEXT'],
    data : [
    ]
});

//************************************数据源***************************************** 
 
 function gopjsd(sid)
 {
      CS('CZCLZ.TpListMsgOp.pjsd', function(retVal) {
        if(retVal)
        {
            
            pjsdstore.loadData(retVal,true);
            Ext.getCmp("pjsd").setValue('');
        }
    }, CS.onError,sid);
 }
 
 
 function save()
 {
    var dq= Ext.getCmp('cx_qy').value;
    var pjsd= Ext.getCmp('pjsd').value;
    var text= Ext.getCmp('dcnr').value;
    var pj= Ext.getCmp('pj').value;
    
    if(pjsd!="")
    {
    
     CS('CZCLZ.TpListMsgOp.savepost', function(retVal) {
        alert("保存成功");
    }, CS.onError,dq,pjsd,text,pj);
    }else
    {
    alert("平价商店不能为空");
    }
 }
 
 //**************************************88
 
Ext.require([
    'Ext.form.*',
    'Ext.layout.container.Column',
    'Ext.tab.Panel' 
]);

 
Ext.onReady(function(){

    Ext.QuickTips.init();

    var bd = Ext.getBody();

 
 

    var simple = Ext.create('Ext.form.Panel', {
        url:'save-form.php',
        frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 350,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 75
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%'
        },

        items: [{
            xtype: 'combobox',
            id: 'cx_qy',
            fieldLabel: '所属地区',
              allowBlank: false,
            editable: false,
            store: dqstore,
             width: 150,
            queryMode: 'local',
            displayField: 'TEXT',
            valueField: 'VALUE',
            labelWidth: 70 ,
             listeners: {
               "select": function (comboBox, record, index) {
                        gopjsd(comboBox.getValue());
                        //alert(comboBox.getValue()); //这个是获取value的不是获取到显示在哪里的值
                    }
            }
        },{
            xtype: 'combobox',
            id: 'pjsd',
            fieldLabel: '平价商店名称',
            editable: false,
            store: pjsdstore,
             width: 150,
            queryMode: 'local',
            displayField: 'TEXT',
            valueField: 'VALUE',
            labelWidth: 70  
        },{
           
           xtype: 'textareafield',
                  fieldLabel: '调查反映问题内容 ',
                    id: 'dcnr',
                    labelAlign: 'top',
                    flex: 1,
                    margins: '0',
                    allowBlank: false
        }, {
            xtype: 'radiogroup',
             id: 'pj',
                fieldLabel: '总体平价',
                columns: 3,
                defaults: {
                    name: 'rating' //Each radio has the same name so the browser will make sure only one is checked at once
                },
                items: [{
                    inputValue: '0',
                    boxLabel: '满意'
                }, {
                    inputValue: '1',
                    boxLabel: '一般满意'
                }, {
                    inputValue: '2',
                    boxLabel: '不满意'
                }]
        }],

        buttons: [{
            text: '保存',
           handler: function() {
                save();
            }
        }]
    });

    simple.render(document.body);
    
       CS('CZCLZ.PjsdDBClass.GetQy', function(retVal) {
        if(retVal)
        {
            dqstore.add([{'VALUE': '', 'TEXT': '所有区域'}]);
            dqstore.loadData(retVal,true);
            Ext.getCmp("cx_qy").setValue('');
        }
    }, CS.onError);


     
});


