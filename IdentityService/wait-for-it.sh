#!/usr/bin/env bash
# wait-for-it.sh - Wait for a service to be available

set -e

host="$1"
port="$2"
shift 2
cmd="$@"

timeout="${TIMEOUT:-60}"
start_ts=$(date +%s)

echo "Waiting for $host:$port..."

while :; do
    nc -z "$host" "$port" && break

    now_ts=$(date +%s)
    elapsed=$((now_ts - start_ts))

    if [ "$elapsed" -ge "$timeout" ]; then
        echo "Timeout occurred after waiting ${timeout}s for $host:$port"
        exit 1
    fi

    sleep 1
done

echo "$host:$port is available after ${elapsed}s"
exec $cmd
