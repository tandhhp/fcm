import { getArguments } from '@/services/work-content';
import { ProForm } from '@ant-design/pro-components';
import EditorJS from '@editorjs/editorjs';
import { useParams } from '@umijs/max';
import React, { useEffect, useRef } from 'react';
import { EDITOR_JS_TOOLS } from './tool';

const BlockEditorForm: React.FC = () => {
  const { id } = useParams();
  const formRef = ProForm.useFormInstance();
  const ejInstance = useRef<any>();

  const initEditor = () => {
    getArguments(id).then((response: any) => {
      const editor = new EditorJS({
        holder: 'editorjs',
        data: response,
        onReady: () => {
          ejInstance.current = editor;
        },
        onChange: (api) => {
          api.saver.save().then((outputData) => {
            formRef?.setFieldValue('blockEditor', outputData);
          });
        },
        autofocus: true,
        tools: EDITOR_JS_TOOLS,
      });
    });
  };

  useEffect(() => {
    if (!ejInstance.current) {
      initEditor();
    }
    return () => {
      ejInstance.current.destroy();
      ejInstance.current = null;
    };
  }, []);

  return (
    <ProForm.Item name="blockEditor">
      <div id="editorjs"> </div>
    </ProForm.Item>
  );
};

export default BlockEditorForm;
