namespace ServiceAbstraction.Contracts.Authentication;


public record ResendConfirmationEmailRequest(
    string Email
);