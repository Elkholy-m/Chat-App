namespace ChatApp.Application.Exceptions;

public class NotFoundException(string msg) : Exception(msg) { }

public class BadRequestException(string msg) : Exception(msg) { }

public class UnauthorizedException(string msg) : Exception(msg) { }
