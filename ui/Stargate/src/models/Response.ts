export interface BaseResponse {
    success: boolean;
    message: string;
}

export interface AddPersonDialogData {
    isNew: boolean;
    existingName: string
};