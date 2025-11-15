export const DEFAULT_NAME = 'Umi Max';

export enum CatalogType {
  Default,
  Article,
  Product,
  Tour = 14,
  Entry = 4,
  Healthcare = 15,
  Hospital = 16,
  Room = 17,
  // Đặc biệt
  Special = 18
}

export enum ESortOrder {
  //
  // Summary:
  //     The default. No sort order is specified.
  //
  // Returns:
  //     -1
  //
  // Value:
  //     -1
  Unspecified = -1,
  //
  // Summary:
  //     Rows are sorted in ascending order.
  //
  // Returns:
  //     0
  //
  // Value:
  //     0
  Ascending,
  //
  // Summary:
  //     Rows are sorted in descending order.
  //
  // Returns:
  //     1
  //
  // Value:
  //     1
  Descending
}