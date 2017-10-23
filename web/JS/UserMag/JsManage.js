var zt;
//************************************数据源*****************************************
var JsStore = Ext.create('Ext.data.Store', {
    fields: [
       {name: 'JS_ID'},
       {name: 'JS_NAME'},
       {name: 'JS_Type'},
       {name: 'JS_ZT'},
       {name: 'JS_PX'}
    ]
});

var ztstore = Ext.create('Ext.data.Store', {
    fields: ['RWLB_ZT', 'text'],
    data : [
        {"RWLB_ZT":'', "text":"全部"},
        {"RWLB_ZT":0, "text":"正常"},
        {"RWLB_ZT":1, "text":"停用"}
    ]
});

var typestore=Ext.create('Ext.data.Store', {
    fields: ['JS_Type', 'text'],
    data : [
        {"JS_Type":0, "text":"监管人员"},
        {"JS_Type":1, "text":"普通人员"}
    ]
});
//************************************数据源*****************************************

//************************************页面方法***************************************
function getJs(zt)
{
    CS('CZCLZ.JsGlClass.GetJs', function(retVal) {
        if(retVal)
        {
            JsStore.loadData(retVal);
        }
        
    },CS.onError ,zt);
}


function xg(id){
    var r=JsStore.findRecord("JS_ID",id).data;
    var win=new addWin();
    win.show(null,function(){
        win.setTitle("角色修改");
        var form=Ext.getCmp('addform');
        form.form.setValues(r); 
        if(r["JS_ZT"]==1)
        {
            Ext.getCmp("JS_ZT").setValue(true);
        }
    });
}
//************************************页面方法***************************************

//************************************弹出界面***************************************
Ext.define('addWin', {
    extend: 'Ext.window.Window',

    height: 250,
    width: 400,
    layout: {
        type: 'fit'
    },
    closeAction:'destroy',
    modal:true,
    title: '新增角色',

    initComponent: function() {
        var me = this;
        me.items = [
            {
                xtype: 'form',
                id:'addform',
                frame: true,
                bodyPadding: 10,
                title: '',
                items: [
                    {
                        xtype: 'textfield',
                        fieldLabel: '角色ID',
                        id:'JS_ID',
                        name:'JS_ID',
                        labelWidth: 70,
                        hidden:true,
                        anchor: '100%'
                    },
                    {
                        xtype: 'textareafield',
                        id:'JS_NAME',
                        name:'JS_NAME',
                        fieldLabel: '名称',
                        labelWidth: 70,
                        allowBlank: false,
                        anchor: '100%'
                    },
                    {
                        xtype: 'numberfield',
                        id:'JS_PX',
                        name:'JS_PX',
                        fieldLabel: '排序',
                        labelWidth: 70,
                        value:0,
                        allowBlank: false,
                        anchor: '100%'
                    },
                    {
                        xtype: 'combobox',
                        id:'jslx',
                        width: 160,
                        fieldLabel: '类型',
                        editable:false,
                        labelWidth: 50,
                        store:typestore,
                        queryMode: 'local',
                        displayField: 'text',
                        valueField: 'JS_Type',
                        value:0
                    },
                    {
                        xtype: 'container',
                        items: [
                            {
                                xtype: 'checkboxfield',
                                id:'JS_ZT',
                                name:'JS_ZT',
                                fieldLabel: '',
                                labelWidth: 70,
                                boxLabel: '停用'
                            }
                        ]
                    }
                ],
                buttonAlign:'center',
                buttons:[
                    {
                        text: '确定',
                        handler: function() {
                            var form=Ext.getCmp('addform');
                            if (form.form.isValid())
                            {
                                //取得表单中的内容
                                var values = form.form.getValues(false);
                                if(Ext.getCmp("JS_ZT").value)
                                {
                                    values["JS_ZT"]=1;
                                }
                                else
                                {
                                    values["JS_ZT"]=0;
                                }
                                
                                values["JS_Type"]=Ext.getCmp("jslx").value;
                                
                                var me=this;
                                
                                CS('CZCLZ.JsGlClass.SaveJs', function(retVal) {
                                    if(retVal)
                                    {
                                        JsStore.loadData(retVal);
                                    }
                                    me.up('window').close()
                                },CS.onError ,values,zt);
                            }
                        }
                    },
                    {
                        text: '取消',
                        handler: function() {
                            this.up('window').close();
                        }
                    }
                ]
            }
        ];
        me.callParent(arguments);
    }
});

//************************************弹出界面***************************************

//************************************主界面*****************************************
Ext.onReady(function() {
    Ext.define('JsView', {
        extend: 'Ext.container.Viewport',

        layout: {
            type: 'fit'
        },

        initComponent: function() {
            var me = this;
            me.items = [
                {
                    xtype: 'gridpanel',
                    id:'JsGrid',
                    store:JsStore,
                    selModel: Ext.create('Ext.selection.CheckboxModel', {

                    }),
                    columns: [Ext.create('Ext.grid.RowNumberer'),
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'JS_NAME',
                            sortable: false,
                            menuDisabled: true,
                            width:400,
                            text: '角色名称'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'JS_ZT',
                            sortable: false,
                            menuDisabled: true,
                            text: '状态',
                            renderer : function(value, cellmeta, record, rowIndex, columnIndex, store){                                  var str="";                                switch(value)                                {                                    case 0:                                        str="正常";                                        break;                                    case 1:                                        cellmeta.style='color: red ';                                        str="停用";                                        break;                                 }                                return str;                              }
                        },
                        {
                            xtype: 'gridcolumn',
                            sortable: false,
                            menuDisabled: true,
                            text: '操作',
                            renderer : function(value, cellmeta, record, rowIndex, columnIndex, store){ 
                                var r=record.data;
                                var id=r["JS_ID"];
                                return "<a href='JavaScript:void(0)' onclick='xg(\""+id+"\")'>修改</a>";
                            }
                        }
                    ],
                    viewConfig: {

                    },
                    dockedItems: [
                        {
                            xtype: 'toolbar',
                            dock: 'top',
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    title: '',
                                    items: [
                                        {
                                            xtype: 'button',
                                            iconCls: 'add',
                                            text: '新增',
                                            handler:function(){
                                                var win=new addWin();
                                                win.show();
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'buttongroup',
                                    title: '',
                                    items: [
                                        {
                                            xtype: 'button',
                                            iconCls: 'delete',
                                            text: '删除',
                                            handler:function(){
                                                var idlist = [];
                                                var grid=Ext.getCmp("JsGrid");
                                                var rds = grid.getSelectionModel().getSelection();
                                                if (rds.length == 0) {
                                                    Ext.Msg.show({
                                                         title:'提示',
                                                         msg: '请选择至少一条要删除的记录!',
                                                         buttons: Ext.MessageBox.OK,
                                                         icon:  Ext.MessageBox.INFO
                                                    });
                                                    return;
                                                }
                                                
                                                Ext.MessageBox.confirm("提示","是否删除你所选?",function(obj){
                                                    if(obj=="yes")
                                                    {
                                                        for (var n = 0, len = rds.length; n < len; n++) {
                                                            var rd = rds[n];
                                                            
                                                            idlist.push(rd.get("JS_ID"));
                                                        }
                                                        
                                                        CS('CZCLZ.JsGlClass.DeleteJs', function(retVal) {
                                                            if(retVal)
                                                            {
                                                                JsStore.loadData(retVal);
                                                            }
                                                        }, CS.onError, idlist,zt);
                                                    }
                                                    else
                                                    {
                                                        return;
                                                    }
                                                });
                                                
                                                
                                            }
                                        }
                                    ]
                                },'->',
                                {
                                    xtype: 'combobox',
                                    id:'ylzt',
                                    width: 160,
                                    fieldLabel: '状态',
                                    editable:false,
                                    labelWidth: 50,
                                    store:ztstore,
                                    queryMode: 'local',
                                    displayField: 'text',
                                    valueField: 'RWLB_ZT',
                                    value:'',
                                    listeners:{
                                         'select': function(field , value , options){
                                            getJs(field.value);
                                         }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ];
            me.callParent(arguments);
        }
    });
    
    new JsView();
    
    zt=Ext.getCmp("ylzt").value;
    
    getJs(zt);
})
//************************************主界面*****************************************