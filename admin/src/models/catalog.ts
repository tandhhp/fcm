import { useState } from 'react';

const useCatalog = () => {
  const [catalogId, setCatalogId] = useState<string>('');
  return {
    catalogId,
    setCatalogId,
  };
};

export default useCatalog;
