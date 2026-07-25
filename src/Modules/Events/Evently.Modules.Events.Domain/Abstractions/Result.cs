using System.Diagnostics.CodeAnalysis;
using Evently.Modules.Events.Domain.Abstractions.Errors;

namespace Evently.Modules.Events.Domain.Abstractions;

/// <summary>
/// Represents the result of an operation that does not return a value.
/// </summary>
/// <remarks>
/// A result can be either successful or failed.
///
/// A successful result must contain <see cref="Error.None"/>, while a failed
/// result must contain a meaningful <see cref="Error"/>.
/// </remarks>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="isSuccess">
    /// A value indicating whether the operation succeeded.
    /// </param>
    /// <param name="error">
    /// The error associated with the result.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the result state and error are inconsistent.
    /// </exception>
    /// <remarks>
    /// A successful result must use <see cref="Error.None"/>.
    ///
    /// A failed result must contain an error other than
    /// <see cref="Error.None"/>.
    /// </remarks>
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    /// <value>
    /// <see langword="true"/> when the operation succeeded;
    /// otherwise, <see langword="false"/>.
    /// </value>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    /// <value>
    /// <see langword="true"/> when the operation failed;
    /// otherwise, <see langword="false"/>.
    /// </value>
    /// <remarks>
    /// This property is the inverse of <see cref="IsSuccess"/>.
    /// </remarks>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error associated with the result.
    /// </summary>
    /// <value>
    /// <see cref="Error.None"/> when the operation succeeded;
    /// otherwise, the error describing the failure.
    /// </value>
    public Error Error { get; }

    /// <summary>
    /// Creates a successful result that does not contain a value.
    /// </summary>
    /// <returns>
    /// A successful <see cref="Result"/>.
    /// </returns>
    public static Result Success() =>
        new(
            isSuccess: true,
            error: Error.None);

    /// <summary>
    /// Creates a successful result containing a value.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of value returned by the successful operation.
    /// </typeparam>
    /// <param name="value">
    /// The value produced by the successful operation.
    /// </param>
    /// <returns>
    /// A successful <see cref="Result{TValue}"/> containing
    /// <paramref name="value"/>.
    /// </returns>
    public static Result<TValue> Success<TValue>(TValue value) =>
        new(
            value: value,
            isSuccess: true,
            error: Error.None);

    /// <summary>
    /// Creates a failed result that does not contain a value.
    /// </summary>
    /// <param name="error">
    /// The error describing why the operation failed.
    /// </param>
    /// <returns>
    /// A failed <see cref="Result"/> containing the specified error.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="error"/> is <see cref="Error.None"/>.
    /// </exception>
    public static Result Failure(Error error) =>
        new(
            isSuccess: false,
            error: error);

    /// <summary>
    /// Creates a failed result that would otherwise contain a value.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of value that the operation would have returned if successful.
    /// </typeparam>
    /// <param name="error">
    /// The error describing why the operation failed.
    /// </param>
    /// <returns>
    /// A failed <see cref="Result{TValue}"/> containing no value and the
    /// specified error.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="error"/> is <see cref="Error.None"/>.
    /// </exception>
    public static Result<TValue> Failure<TValue>(Error error) =>
        new(
            value: default,
            isSuccess: false,
            error: error);
}

/// <summary>
/// Represents the result of an operation that can either succeed with a value
/// of type <typeparamref name="TValue"/> or fail with an <see cref="Error"/>.
/// </summary>
/// <typeparam name="TValue">
/// The type of value returned when the operation succeeds.
/// </typeparam>
public class Result<TValue> : Result
{
    /// <summary>
    /// Stores the value associated with a successful result.
    /// </summary>
    /// <remarks>
    /// This field is nullable because a failed result does not contain a value.
    ///
    /// The stored value should only be accessed through the
    /// <see cref="Value"/> property.
    /// </remarks>
    private readonly TValue? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> class.
    /// </summary>
    /// <param name="value">
    /// The value associated with the result.
    /// </param>
    /// <param name="isSuccess">
    /// A value indicating whether the operation succeeded.
    /// </param>
    /// <param name="error">
    /// The error associated with the result.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown by the base constructor when the result state and error are
    /// inconsistent.
    /// </exception>
    /// <remarks>
    /// When <paramref name="isSuccess"/> is <see langword="true"/>,
    /// <paramref name="value"/> is expected to be non-null and
    /// <paramref name="error"/> must be <see cref="Error.None"/>.
    ///
    /// When <paramref name="isSuccess"/> is <see langword="false"/>,
    /// the value is normally <see langword="null"/> or the default value of
    /// <typeparamref name="TValue"/>.
    /// </remarks>
    public Result(
        TValue? value,
        bool isSuccess,
        Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the value produced by a successful operation.
    /// </summary>
    /// <value>
    /// The value associated with the successful result.
    /// </value>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the result represents a failure.
    /// </exception>
    /// <remarks>
    /// The null-forgiving operator tells the compiler that the value is
    /// expected to be non-null when the result is successful.
    ///
    /// It does not perform a runtime null check.
    /// </remarks>
    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException(
            "The value of a failure result cannot be accessed.");

    /// <summary>
    /// Converts a nullable value of type <typeparamref name="TValue"/> into a
    /// <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="value">
    /// The value to convert.
    /// </param>
    /// <returns>
    /// A successful result when <paramref name="value"/> is not null;
    /// otherwise, a failed result containing <see cref="Error.NullValue"/>.
    /// </returns>
    /// <remarks>
    /// This implicit conversion allows a value to be assigned or returned
    /// wherever a <see cref="Result{TValue}"/> is expected.
    ///
    /// For example:
    /// <code>
    /// Event? eventEntity = await repository.GetAsync(id);
    /// Result&lt;Event&gt; result = eventEntity;
    /// </code>
    /// </remarks>
    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null
            ? Success(value)
            : Failure<TValue>(Error.NullValue);

    /// <summary>
    /// Creates a failed result representing a validation failure.
    /// </summary>
    /// <param name="error">
    /// The validation error associated with the result.
    /// </param>
    /// <returns>
    /// A failed <see cref="Result{TValue}"/> containing no value and the
    /// specified validation error.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="error"/> is <see cref="Error.None"/>.
    /// </exception>
    public static Result<TValue> ValidationFailure(Error error) =>
        new(
            value: default,
            isSuccess: false,
            error: error);
}
