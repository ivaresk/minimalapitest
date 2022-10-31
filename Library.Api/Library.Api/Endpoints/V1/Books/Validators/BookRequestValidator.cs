using FastEndpoints;
using FluentValidation;
using Library.Api.Endpoints.V1.Books.Models;

namespace Library.Api.Endpoints.V1.Books.Validators;

public class BookRequestValidator : Validator<BookRequest>
{
	public BookRequestValidator()
	{
		RuleFor(b => b.Isbn)
			.Matches("^(?=(?:\\D*\\d){10}(?:(?:\\D*\\d){3})?$)[\\d-]+$")
			.WithMessage("the ISBN is not in the correct format");

		RuleFor(b => b.Author)
			.NotEmpty()
			.WithMessage("Author is requied");

		RuleFor(b=> b.Title)
            .NotEmpty()
            .WithMessage("Title is requied");

        RuleFor(b => b.ShortDescription)
            .NotEmpty()
            .WithMessage("Description is requied");

        RuleFor(b => b.PageCount)
            .GreaterThan(0)
            .WithMessage("Page count is requied");

    }
}
