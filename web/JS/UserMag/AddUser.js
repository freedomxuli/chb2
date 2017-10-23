var Up_userid = queryString.id; 

//************************************数据源*****************************************
var dqstore = Ext.create('Ext.data.Store', {
    fields: ['VALUE', 'TEXT'],
    data: [
    ]
});
//************************************主界面*****************************************

Ext.onReady(function() {
    Ext.define('useraddview', {
        extend: 'Ext.container.Viewport',

        layout: {
            type: 'fit'
        },

        initComponent: function() {
            var me = this;
            me.items = [
                {
                    xtype: 'form',
                    id: 'addform',
                    autoScroll: true,
                    layout: {
                        columns: 2,
                        type: 'table'
                    },
                    bodyPadding: 10,
                    title: '新增',
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'ID',
                            id: 'User_ID',
                            name: 'User_ID',
                            labelWidth: 70,
                            hidden: true,
                            colspan: 2
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '用户名',
                            id: 'LoginName',
                            name: 'LoginName',
                            labelWidth: 70,
                            allowBlank: false,
                            anchor: '100%'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '密码',
                            id: 'Password',
                            name: 'Password',
                            labelWidth: 70,
                            allowBlank: false,
                            anchor: '100%'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '单位名称',
                            id: 'User_DM',
                            name: 'User_DM',
                            labelWidth: 70,
                            anchor: '100%'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '联系人',
                            id: 'User_XM',
                            name: 'User_XM',
                            labelWidth: 70,
                            anchor: '100%'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '职务',
                            id: 'User_ZW',
                            name: 'User_ZW',
                            labelWidth: 70,
                            anchor: '100%'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '电话',
                            id: 'User_DH',
                            name: 'User_DH',
                            labelWidth: 70,
                            anchor: '100%'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '手机',
                            id: 'User_SJ',
                            name: 'User_SJ',
                            labelWidth: 70,
                            anchor: '100%'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '电子邮件',
                            id: 'User_Email',
                            name: 'User_Email',
                            labelWidth: 70,
                            anchor: '100%'
                        },
                        {
                            xtype: 'combobox',
                            id: 'QY_ID',
                            name: 'QY_ID',
                            fieldLabel: '所属地区',
                            editable: false,
                            store: dqstore,
                            queryMode: 'local',
                            displayField: 'TEXT',
                            valueField: 'VALUE',
                            labelWidth: 70,
                            anchor: '100%'
                        },
                        {
                            xtype: 'label',
                            id: 'lab',
                            name: 'lab',
                            anchor: '100%'
                        },
                        {
                            xtype: 'textareafield',
                            width: 585,
                            fieldLabel: '地址',
                            colspan: 2,
                            id: 'User_DZ',
                            name: 'User_DZ',
                            labelWidth: 70,
                            anchor: '100%'
                        },
                        {
                            xtype: 'checkboxfield',
                            fieldLabel: '状态',
                            boxLabel: '停用',
                            labelWidth: 70,
                            colspan: 2,
                            id: 'User_Enable',
                            name: 'User_Enable'
                        },
                        {
                            xtype: 'checkboxgroup',
                            id: 'yhjsGroup',
                            width: 400,
                            layout: {
                                type: 'table'
                            },
                            fieldLabel: '角色',
                            labelWidth: 70,
                            colspan: 2,
                            items: [
                            ]
                        },
                        {
                            xtype: 'tabpanel',
                            id: 'jstab',
                            height: 300,
                            activeTab: 1,
                            colspan: 2,
                            items: [
                            ]
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确定',
                            iconCls: 'dropyes',
                            handler: function() {
                                var form = Ext.getCmp('addform');
                                if (form.form.isValid()) {
                                    //取得表单中的内容
                                    var values = form.form.getValues(false);

                                    if (Ext.getCmp("User_Enable").value) {
                                        values["User_Enable"] = 1;
                                    }
                                    else {
                                        values["User_Enable"] = 0;
                                    }

                                    var yhjgs = Ext.getCmp("yhjsGroup").items.items;
                                    var yhjsids = [];
                                    var yhjsDwids = [];
                                    var qxids=[];
                                    for (var i = 0; i < yhjgs.length; i++) {
                                        if (yhjgs[i].checked) {
                                            var num = yhjsids.length;
                                            yhjsids[yhjsids.length] = yhjgs[i].inputValue;

                                            var tabgrid = Ext.getCmp("tab" + yhjgs[i].inputValue);
                                            var rds = tabgrid.getSelectionModel().getSelection();

                                            var idlist = [];
                                            for (var n = 0, len = rds.length; n < len; n++) {
                                                var rd = rds[n];
                                                idlist.push(rd.get("ID"));
                                            }
                                            if (idlist.length == 0) {
                                                Ext.Msg.show({
                                                    title: '提示',
                                                    msg: '请至少选择一条数据!',
                                                    buttons: Ext.MessageBox.OK,
                                                    icon: Ext.MessageBox.INFO
                                                });
                                                return;
                                            }

                                            yhjsDwids[num] = idlist;
                                            
                                            var qxtab=Ext.getCmp("qx" + yhjgs[i].inputValue);
                                            if(qxtab)
                                            {
                                                var qxrds = qxtab.getSelectionModel().getSelection();
                                                for (var m = 0, qxlen = qxrds.length; m < qxlen; m++) {
                                                    var qxrd = qxrds[m];
                                                    qxids.push(qxrd.get("ID"));
                                                }
                                            }
                                        }
                                    }

                                    if (yhjsids.length == 0) {
                                        Ext.Msg.show({
                                            title: '提示',
                                            msg: '请选择角色类型!',
                                            buttons: Ext.MessageBox.OK,
                                            icon: Ext.MessageBox.INFO
                                        });
                                        return;
                                    }

                                    CS('CZCLZ.YHGLClass.SaveUser', function(retVal) {
                                        if (retVal) {
                                            history.back();
                                        }
                                    }, CS.onError, values, yhjsids, yhjsDwids,qxids);

                                }
                            }
                        },
                        {
                            text: '返回',
                            iconCls: 'back',
                            handler: function() {
                                history.back();
                            }
                        }
                    ]
                }
            ];
            me.callParent(arguments);
        }
    });

    new useraddview();
    
    CS('CZCLZ.PjsdDBClass.GetQy', function(retVal) {
        if (retVal) {
            dqstore.add([{ 'VALUE': '', 'TEXT': '所有区域'}]);
            dqstore.loadData(retVal, true);
            Ext.getCmp("QY_ID").setValue('');
        }
    }, CS.onError);

    CS('CZCLZ.YHGLClass.GetJs', function(retVal) {
        if (retVal) {
            var Items = [];
            for (var i = 0; i < retVal.length; i++) {

                var checkbox = new Ext.form.field.Checkbox({
                    xtype: 'checkboxfield',
                    boxLabel: retVal[i].JS_NAME,
                    name: 'yhjs',
                    hideLabel: true,
                    inputValue: retVal[i].JS_ID,
                    handler: function(chk, checked) {
                        var jsid = this.inputValue;
                        var jsname = this.boxLabel;
                        if (checked) {
                            CS('CZCLZ.YHGLClass.GetDWAndGl', function(retVal) {
                                var DWStore;
                                var jspan;
                                if(jsid.toLowerCase()=="7E53492E-CF66-411F-83C4-7923467F59B4".toLowerCase())
                                {
                                    DWStore = Ext.create('Ext.data.Store', {
                                        fields: [
                                           { name: 'ID' },
                                           { name: 'MC' },
                                           { name: 'QY_NAME' }
                                        ]
                                    });
                                }
                                else
                                {
                                    DWStore = Ext.create('Ext.data.Store', {
                                        fields: [
                                           { name: 'ID' },
                                           { name: 'MC' }
                                        ]
                                    });
                                }
                                DWStore.loadData(retVal.dtdw);
                                if(jsid.toLowerCase()=="7E53492E-CF66-411F-83C4-7923467F59B4".toLowerCase())
                                {
                                    jspan = Ext.create('Ext.grid.Panel', {
                                        id: 'tab' + jsid,
                                        title: jsname,
                                        store: DWStore,
                                        stateful: false,
                                        layout: {
                                            type: 'fit'
                                        },
                                        selModel: Ext.create('Ext.selection.CheckboxModel', {

                                        }),
                                        columns: [
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'QY_NAME',
                                                sortable: false,
                                                menuDisabled: true,
                                                width: 100,
                                                text: '区域'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'MC',
                                                sortable: false,
                                                menuDisabled: true,
                                                width: 200,
                                                text: '名称'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'ID',
                                                hidden: true,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: 'ID'
                                            }
                                        ],
                                        viewConfig: {

                                        }
                                    });
                                }
                                else
                                {
                                    jspan = Ext.create('Ext.grid.Panel', {
                                        id: 'tab' + jsid,
                                        title: jsname,
                                        store: DWStore,
                                        stateful: false,
                                        layout: {
                                            type: 'fit'
                                        },
                                        selModel: Ext.create('Ext.selection.CheckboxModel', {

                                        }),
                                        columns: [
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'MC',
                                                sortable: false,
                                                menuDisabled: true,
                                                width: 200,
                                                text: '名称'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'ID',
                                                hidden: true,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: 'ID'
                                            }
                                        ],
                                        viewConfig: {

                                        }
                                    });
                                }
                                Ext.getCmp("jstab").add(jspan);
                                
                                //判断是否勾选的是权限表里是否有内容
                                var dtqx=retVal.dtqx;
                                var qxpan;
                                if(dtqx.length>0)
                                {
                                    var qxStore = Ext.create('Ext.data.Store', {
                                        fields: [
                                           { name: 'ID' },
                                           { name: 'MC' }
                                        ]
                                    });
                                    qxStore.loadData(dtqx);
                                    qxpan = Ext.create('Ext.grid.Panel', {
                                        id: 'qx' + jsid,
                                        title: jsname+'权限',
                                        store: qxStore,
                                        stateful: false,
                                        layout: {
                                            type: 'fit'
                                        },
                                        selModel: Ext.create('Ext.selection.CheckboxModel', {

                                        }),
                                        columns: [
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'MC',
                                                sortable: false,
                                                menuDisabled: true,
                                                width: 400,
                                                text: '名称'
                                            },
                                            {
                                                xtype: 'gridcolumn',
                                                dataIndex: 'ID',
                                                hidden: true,
                                                sortable: false,
                                                menuDisabled: true,
                                                text: 'ID'
                                            }
                                        ],
                                        viewConfig: {

                                        }
                                    });
                                    Ext.getCmp("jstab").add(qxpan);
                                }
                                
                                Ext.getCmp("jstab").setActiveTab(jspan);
                                var usergl = retVal.usergl;
                                for (var i = 0; i < usergl.length; i++) {
                                    var record = DWStore.findRecord("ID", usergl[i].DW_ID);
                                    jspan.getSelectionModel().select(record, true, true);
                                }
                                
                                if(dtqx.length>0)
                                {
                                    Ext.getCmp("jstab").setActiveTab(qxpan);
                                    var qxgl = retVal.dtqxgl;
                                    if(qxpan.id=="qx7e53492e-cf66-411f-83c4-7923467f59b4")
                                    {
                                        for (var i = 0; i < qxgl.length; i++) {
                                            if(qxgl[i].MODULENAME.substr(0,7)=="平价商店权限_")
                                            {
                                                var record = qxStore.findRecord("ID", qxgl[i].PRIVILEGECODE);
                                                qxpan.getSelectionModel().select(record, true, true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (var i = 0; i < qxgl.length; i++) {
                                            if(qxgl[i].MODULENAME.substr(0,7)!="平价商店权限_")
                                            {
                                                var record = qxStore.findRecord("ID", qxgl[i].PRIVILEGECODE);
                                                qxpan.getSelectionModel().select(record, true, true);
                                            }
                                        }
                                    }
                                }

                            }, CS.onError, jsid, Up_userid)

                        }
                        else {
                            Ext.getCmp("jstab").remove(Ext.getCmp("tab" + jsid))
                            if(Ext.getCmp("qx" + jsid))
                            {
                                Ext.getCmp("jstab").remove(Ext.getCmp("qx" + jsid))
                            }
                        }
                    }
                });
                Items.push(checkbox);
            }
            Ext.getCmp("yhjsGroup").add(Items);

            if (Up_userid) {
                
            
                //根据用户和角色
                CS('CZCLZ.YHGLClass.GetUserAndJs', function(retVal) {
                    if (retVal) {
                        var dtuser = retVal.dtuser;
                        var dtjs = retVal.dtjs;
                        var addform = Ext.getCmp('addform');
                        addform.setTitle("修改");
                        addform.form.setValues(dtuser[0]);

                        var jsids = [];
                        for (var i = 0; i < dtjs.length; i++) {
                            jsids[i] = dtjs[i].JS_ID;
                        }

                        Ext.getCmp("yhjsGroup").setValue({
                            yhjs: jsids
                        });

                    }
                }, CS.onError, Up_userid);


            }
        }
    }, CS.onError);



})

//************************************主界面*****************************************