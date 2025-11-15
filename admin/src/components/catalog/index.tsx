import { addCatalog, listTree, treeDrop } from '@/services/catalog';
import { PlusOutlined } from '@ant-design/icons';
import {
  ProFormInstance,
  ModalForm,
  ProFormCheckbox,
  ProFormText,
} from '@ant-design/pro-components';
import { history } from '@umijs/max';
import { Button, Input, message, Tree } from 'antd';
import { DataNode, TreeProps } from 'antd/lib/tree';
import React, { useState, useEffect, useRef } from 'react';

const { Search } = Input;

const Catalog: React.FC = () => {
  const formRef = useRef<ProFormInstance>();

  const [treeData, setTreeData] = useState<DataNode[]>([]);
  const [expandedKeys, setExpandedKeys] = useState<React.Key[]>([]);
  const [autoExpandParent, setAutoExpandParent] = useState(true);
  const [visible, setVisible] = useState<boolean>(false);

  useEffect(() => {
    listTree().then((response) => {
      setTreeData(response);
    });
  }, []);

  const onDrop: TreeProps['onDrop'] = (info) => {
    console.log(info);
    const dropKey = info.node.key;
    const dragKey = info.dragNode.key;
    const dropPos = info.node.pos.split('-');
    const dropPosition =
      info.dropPosition - Number(dropPos[dropPos.length - 1]);

    const loop = (
      data: DataNode[],
      key: React.Key,
      callback: (node: DataNode, i: number, data: DataNode[]) => void,
    ) => {
      for (let i = 0; i < data.length; i++) {
        if (data[i].key === key) {
          return callback(data[i], i, data);
        }
        if (data[i].children) {
          loop(data[i].children!, key, callback);
        }
      }
    };
    const data = [...treeData];

    // Find dragObject
    let dragObj: DataNode;
    loop(data, dragKey, (item, index, arr) => {
      arr.splice(index, 1);
      dragObj = item;
    });

    if (!info.dropToGap) {
      // Drop on the content
      loop(data, dropKey, (item) => {
        item.children = item.children || [];
        // where to insert 示例添加到头部，可以是随意位置
        item.children.unshift(dragObj);
      });
    } else if (
      ((info.node as any).props.children || []).length > 0 && // Has children
      (info.node as any).props.expanded && // Is expanded
      dropPosition === 1 // On the bottom gap
    ) {
      loop(data, dropKey, (item) => {
        item.children = item.children || [];
        // where to insert 示例添加到头部，可以是随意位置
        item.children.unshift(dragObj);
        // in previous version, we use item.children.push(dragObj) to insert the
        // item to the tail of the children
      });
    } else {
      let ar: DataNode[] = [];
      let i: number;
      loop(data, dropKey, (_item, index, arr) => {
        ar = arr;
        i = index;
      });
      if (dropPosition === -1) {
        ar.splice(i!, 0, dragObj!);
      } else {
        ar.splice(i! + 1, 0, dragObj!);
      }
    }
    const body = {
      dragNodeKey: info.dragNodesKeys[0],
      node: info.node.key,
      dropToGap: info.dropToGap,
    };
    treeDrop(body).then((response) => {
      if (response.succeeded) {
        message.success('Saved!');
      }
    });
    setTreeData(data);
  };

  const onExpand = (newExpandedKeys: React.Key[]) => {
    setExpandedKeys(newExpandedKeys);
    setAutoExpandParent(false);
  };

  const onSelect = (selectedKeys: React.Key[]) => {
    if (!selectedKeys || selectedKeys.length === 0) {
      message.warning('Can de-select');
      return;
    }
    formRef.current?.setFieldValue('parentId', selectedKeys[0]);
    history.push(`/catalog/${selectedKeys[0]}`);
  };

  const onFinish = async (values: API.Catalog) => {
    const response = await addCatalog(values);
    if (response.succeeded) {
      message.success('Saved!');
      setVisible(false);
    }
  };

  const handleAdd = () => {
    setVisible(true);
  };

  return (
    <div>
      <div className="flex">
        <Search />
        <Button type="primary" icon={<PlusOutlined />} onClick={handleAdd} />
      </div>
      <div className="bg-white">
        <Tree
          showLine
          showIcon
          treeData={treeData}
          expandedKeys={expandedKeys}
          onExpand={onExpand}
          autoExpandParent={autoExpandParent}
          draggable
          onSelect={onSelect}
          onDrop={onDrop}
        />
      </div>
      <ModalForm
        onOpenChange={setVisible}
        open={visible}
        onFinish={onFinish}
        formRef={formRef}
      >
        <ProFormText name="parentId" label="Parrent Id" disabled />
        <ProFormText name="name" label="Name" />
        <ProFormText name="normalizedName" label="Normalized Name" />
        <ProFormCheckbox name="active" label="Active" />
      </ModalForm>
    </div>
  );
};

export default Catalog;
