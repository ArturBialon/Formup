import { ErrorMessage } from "../../_interfaces/error-message.interface";

export const DEFAULT_ERROR_MESSAGES: ErrorMessage = {
  required: 'This field is required',
  email: 'E-mail address is required',
  minlength: 'Value is too short',
  maxlength: 'Value is too long',
  max: 'Value is to high'
}