export class RefineRidesModel {
  filter: string;
  sort: {
    byDate: string;
    byRating: string;
  }
  search: {
    byDate: {
      from: Date;
      to: Date;
    }
    byRating: {
      from: number;
      to: number;
    }
    byPrice: {
      from: number;
      to: number;
    }
  }
}