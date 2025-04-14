export interface Role {
  id: string;
  name: string;
  description?: string | null;
  createdOn: string;
  createdBy: string;
  updatedOn?: string | null;
  updatedBy?: string | null;
}