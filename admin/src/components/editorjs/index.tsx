import EditorJS from '@editorjs/editorjs';
import React, { useRef, useEffect } from 'react';
import { useParams } from '@umijs/max';
import { getBlockEditor } from '@/services/work-content';
import { EDITOR_JS_TOOLS } from './tool';

type ProEditorBlockProps = {
  onChange: any;
};

const ProEditorBlock: React.FC<ProEditorBlockProps> = (props) => {
  const { id } = useParams();
  const ejInstance = useRef<any>();

  const initEditor = () => {
    getBlockEditor(id).then((response) => {
      const editor = new EditorJS({
        holder: 'editorjs',
        data: {
          time: new Date().getTime(),
          blocks: response,
        },
        onReady: () => {
          ejInstance.current = editor;
        },
        onChange: (api) => {
          api.saver.save().then((outputData) => {
            const data = {
              id,
              ...outputData,
            };
            props.onChange(data);
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
    <React.Fragment>
      <div id="editorjs"> </div>
    </React.Fragment>
  );
};

export default ProEditorBlock;
