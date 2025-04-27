export interface Page {
  id: string;
  name: string;
  url: string;
  createdOn: string;
  createdBy: string;
  updatedOn?: string | null;
  updatedBy?: string | null;
}