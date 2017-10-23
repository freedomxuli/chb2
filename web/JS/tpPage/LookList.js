//**********************************************************数据源**********************************************

  
var UserStore = createSFW4Store({
    data: [],
    fields: [
        { name: 'pd_id', type: 'string' },
        { name: 'PJSD_MC', type: 'string' },
        { name: 'a', type: 'string' },
        { name: 'b1', type: 'string' },
        { name: 'b2', type: 'string' },
        { name: 'b3', type: 'string' },
          { name: 'b1a', type: 'string' },
        { name: 'b2a', type: 'string' },
        { name: 'b3a', type: 'string' }
     ],

    onPageChange: function(sto, nPage, sorters) {
        DataBind();
    }
});

var yfstore = Ext.create('Ext.data.Store', {
    fields: ['yuefen', 'yuefen_value'],
    data: [
          { "yuefen": 0, "yuefen_value": "" },
          { "yuefen": 1, "yuefen_value": "1月" },
         { "yuefen": 2, "yuefen_value": "2月" },
         { "yuefen": 3, "yuefen_value": "3月" },
         { "yuefen": 4, "yuefen_value": "4月" },
         { "yuefen": 5, "yuefen_value": "5月" },
         { "yuefen": 6, "yuefen_value": "6月" },
         { "yuefen": 7, "yuefen_value": "7月" },
         { "yuefen": 8, "yuefen_value": "8月" },
         { "yuefen": 9, "yuefen_value": "9月" },
         { "yuefen": 10, "yuefen_value": "10月" },
         { "yuefen": 11, "yuefen_value": "11月" },
         { "yuefen": 12, "yuefen_value": "12月" }
    ]
});



var JdStore = new Ext.data.Store({
    fields: ['j_num', 'j_value'],
    data: [
        { "j_num": "1", "j_value": "第一季度" },
        { "j_num": "2", "j_value": "第二季度" },
        { "j_num": "3", "j_value": "第三季度" },
        { "j_num": "4", "j_value": "第四季度" }
    ]
});



//年份数据源
var nianfenstore = Ext.create('Ext.data.Store', {
    fields: ['VALUE', 'TEXT'],
    data : [
    ]
});


//**************************************************************************************************************

function DataBind() {
    var jidu = Ext.getCmp('cbjd').value;
    var nianfen = Ext.getCmp('nianfen').value;
    CS('CZCLZ.TpListMsgOp.GetProList', function(retVal) {
        UserStore.setData({
            data: retVal.dt
        });
    }, CS.onError, "", jidu, nianfen);
}
 
 
 function DataNianFen()
{
      CS('CZCLZ.TpListMsgOp.GetNianfen', function(retVal) {
        if(retVal)
        {
            
            nianfenstore.loadData(retVal,true);
            Ext.getCmp("nianfen").setValue('');
        }
    }, CS.onError);
}
 
//**********************************************************主界面**********************************************
Ext.onReady(function() {
    Ext.define('DbTj', {
        extend: 'Ext.container.Viewport',

        layout: {
            type: 'fit'
        },

        initComponent: function() {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'panel',
                        layout: {
                            type: 'fit'
                        },
                        items: [
                            {
                                xtype: 'gridpanel',
                                autoScroll: true,
                                store: UserStore,
                                id: 'cxnr',
                                columns: [
                                    Ext.create('Ext.grid.RowNumberer', {width:40}),
                                    {
                                        dataIndex: 'PJSD_MC',
                                        width: 250,
                                        text: '商店名称',
                                        sortable: false,
                                        menuDisabled: true
                                    },
                                    {
                                        dataIndex: 'a',
                                        width: 65,
                                        align: "right",
                                        text: '调研人数',
                                        sortable: false,
                                        menuDisabled: true
                                    },
                                    {
                                        text: '满意',
                                        id: 'my',
                                        sortable: false,
                                        menuDisabled: true,
                                        columns: [
                                            {
                                                xtype: 'gridcolumn',
                                                text: '人数',
                                                width: 70,
                                                align: "right",
                                                sortable: false,
                                                menuDisabled: false,
                                                dataIndex: 'b1'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                text: '占比(%)',
                                                width: 70,
                                                align: "right",
                                                sortable: false,
                                                menuDisabled: false,
                                                dataIndex: 'b1a'
                                            }
                                        ]
                                    },
                                    {
                                        text: '基本满意',
                                        id: 'jbmy',
                                        sortable: false,
                                        menuDisabled: true,
                                        columns: [
                                            {
                                                xtype: 'gridcolumn',
                                                text: '人数',
                                                width: 70,
                                                align: "right",
                                                sortable: false,
                                                menuDisabled: false,
                                                dataIndex: 'b2'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                text: '占比(%)',
                                                width: 70,
                                                align: "right",
                                                sortable: false,
                                                menuDisabled: false,
                                                dataIndex: 'b2a'
                                            }
                                        ]
                                    },
                                    {
                                        text: '不满意',
                                        id: 'bmy',
                                        sortable: false,
                                        menuDisabled: true,
                                        columns: [
                                            {
                                                xtype: 'gridcolumn',
                                                text: '人数',
                                                width: 70,
                                                align: "right",
                                                sortable: false,
                                                menuDisabled: false,
                                                dataIndex: 'b3'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                text: '占比(%)',
                                                width: 70,
                                                align: "right",
                                                sortable: false,
                                                menuDisabled: false,
                                                dataIndex: 'b3a'
                                            }
                                        ]
                                    },
                                    {
                                        width: 65,
                                        align: "right",
                                        text: '得分',
                                        sortable: false,
                                        menuDisabled: true,
                                        renderer: function(value, cellmeta, record, rowIndex, columnIndex, store) {
                                            var str = (4 * record.data.b1a + 2 * record.data.b2a)/100.0;
                                            return str;
                                        }
                                    }
                                ],
                                viewConfig: {

                            },
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    id: 'tbMonth',
                                    dock: 'top',
                                    items: [
                                            {
                                                xtype: 'combobox',
                                                id: 'nianfen',
                                                fieldLabel: '年份',
                                                width: 100,
                                                store: nianfenstore,
                                                queryMode: 'local',
                                                displayField: 'TEXT',
                                                valueField: 'VALUE',
                                                labelWidth: 40
                                            },
                                            {
                                                xtype: 'combobox',
                                                fieldLabel: '请选择季度',
                                                id: 'cbjd',
                                                width: 150,
                                                labelWidth: 70,
                                                store: JdStore,
                                                displayField: 'j_value',
                                                valueField: 'j_num',
                                                queryMode: 'local'
                                            },
                                            {
                                                xtype: 'buttongroup',
                                                columns: 2,
                                                items: [
                                                    {
                                                        xtype: 'button',
                                                        text: '查询', iconCls: 'search',
                                                        handler: function() {
                                                            DataBind();
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
                                                        iconCls: 'download',
                                                        text: '导出',
                                                        handler:function(){
                                                            var jidu = Ext.getCmp('cbjd').value;
                                                            var nianfen = Ext.getCmp('nianfen').value;
                                                            var filename="";
                                                            if(nianfen!="")
                                                            {
                                                                filename+=nianfen;
                                                            }
                                                            if(jidu!=null)
                                                            {
                                                                filename+=jidu;
                                                            }
                                                            
                                                            DownloadFile("CZCLZ.TpListMsgOp.GetProListTofile",filename+"问卷调查考核.xls","",jidu,nianfen);
                                                        }
                                                    }
                                                ]
                                            }
                                            
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            });
            me.callParent(arguments);
        }
    });
    new DbTj();
    DataBind();
    DataNianFen();
});